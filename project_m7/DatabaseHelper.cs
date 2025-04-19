using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace project_m7
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));
            }
            this.connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Database connection successful!");
                    
                    // Create accounts table if it doesn't exist
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'accounts')
                        BEGIN
                            CREATE TABLE accounts (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                card_number NVARCHAR(50) UNIQUE NOT NULL,
                                PasswordHash NVARCHAR(100) NOT NULL,
                                CreatedAt DATETIME DEFAULT GETDATE()
                            )
                            PRINT 'Accounts table created successfully'
                        END
                        ELSE
                        BEGIN
                            PRINT 'Accounts table already exists'
                        END";

                    using (var command = new SqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Table creation query executed");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw; // Re-throw to handle in the calling code
            }
        }

        public bool RegisterUser(string cardNumber, string password)
        {
            try
            {
                string passwordHash = HashPassword(password);

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO accounts (card_number, PasswordHash)
                        VALUES (@cardNumber, @PasswordHash)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@card_number", cardNumber);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        int result = command.ExecuteNonQuery();
                        Console.WriteLine($"Registration result: {result} rows affected");
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RegisterUser: {ex.Message}");
                return false;
            }
        }

        public bool LoginUser(string cardNumber, string password)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT PasswordHash
                        FROM accounts
                        WHERE card_number = @card_number";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@card_number", cardNumber);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader["PasswordHash"].ToString();
                                string inputHash = HashPassword(password);
                                bool result = storedHash == inputHash;
                                Console.WriteLine($"Login attempt for card {cardNumber}: {result}");
                                return result;
                            }
                            else
                            {
                                Console.WriteLine($"No account found with card number: {cardNumber}");
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoginUser: {ex.Message}");
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 