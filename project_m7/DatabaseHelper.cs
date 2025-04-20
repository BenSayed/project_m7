using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace project_m7
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class DatabaseHelper
    {
        private readonly string connectionString = "Data Source=HASSAN;Initial Catalog=Bank_app;Integrated Security=True;TrustServerCertificate=True";

        public DatabaseHelper()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'accounts')
                    BEGIN
                        CREATE TABLE accounts (
                            [cardnum] VARCHAR(16) PRIMARY KEY,
                            [password] NVARCHAR(100) NOT NULL,
                            [balance] DECIMAL(18,2) NOT NULL DEFAULT 0
                        )
                    END
                    ELSE
                    BEGIN
                        -- Check if balance column exists, if not add it
                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('accounts') AND name = 'balance')
                        BEGIN
                            ALTER TABLE accounts ADD [balance] DECIMAL(18,2) NOT NULL DEFAULT 0
                        END
                    END";
                using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32);
            }
        }

        public AuthResult RegisterUser(string cardnum, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if card number already exists
                    string checkQuery = "SELECT COUNT(*) FROM accounts WHERE cardnum = @cardnum";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@cardnum", cardnum);
                        int count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            return new AuthResult { Success = false, Message = "Card number already exists." };
                        }
                    }

                    // Insert new user with default balance of 0
                    string insertQuery = @"
                        INSERT INTO accounts (cardnum, password, balance)
                        VALUES (@cardnum, @password, 0)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@cardnum", cardnum);
                        command.Parameters.AddWithValue("@password", password);
                        command.ExecuteNonQuery();
                    }

                    return new AuthResult { Success = true, Message = "Registration successful." };
                }
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = "Registration failed: " + ex.Message };
            }
        }

        public AuthResult LoginUser(string cardnum, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT password, balance FROM accounts WHERE cardnum = @cardnum";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cardnum", cardnum);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["password"].ToString();
                                if (password == storedPassword)
                                {
                                    return new AuthResult { Success = true, Message = "Login successful." };
                                }
                                else
                                {
                                    return new AuthResult { Success = false, Message = "Invalid password." };
                                }
                            }
                            else
                            {
                                return new AuthResult { Success = false, Message = "Card number not found." };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = "Login failed: " + ex.Message };
            }
        }

        public decimal GetBalance(string cardnum)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT balance FROM accounts WHERE cardnum = @cardnum";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cardnum", cardnum);
                        object result = command.ExecuteScalar();
                        return result != null ? Convert.ToDecimal(result) : 0;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool UpdateBalance(string cardnum, decimal newBalance)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE accounts SET balance = @balance WHERE cardnum = @cardnum";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@balance", newBalance);
                        command.Parameters.AddWithValue("@cardnum", cardnum);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool VerifyCardNumber(string cardnum)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM accounts WHERE cardnum = @cardnum";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cardnum", cardnum);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error verifying card number: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UpdatePassword(string cardnum, string newPassword)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE accounts SET password = @password WHERE cardnum = @cardnum";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@password", newPassword);
                        command.Parameters.AddWithValue("@cardnum", cardnum);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}