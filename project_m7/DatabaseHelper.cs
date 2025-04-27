using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace project_m7
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class Transaction
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime Date { get; set; }
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
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string createAccountsQuery = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'accounts')
                            BEGIN
                                CREATE TABLE accounts (
                                    cardnum NVARCHAR(50) PRIMARY KEY,
                                    password NVARCHAR(100) NOT NULL,
                                    balance DECIMAL(18,2) NOT NULL DEFAULT 0
                                )
                            END";
                        using (SqlCommand command = new SqlCommand(createAccountsQuery, connection))
                        {
                            command.ExecuteNonQuery();
                    }
                    
                        string createTransactionsQuery = @"
                            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'transactions')
                            BEGIN
                                CREATE TABLE transactions (
                                    id INT IDENTITY(1,1) PRIMARY KEY,
                                    cardnum NVARCHAR(50) NOT NULL,
                                    type NVARCHAR(50) NOT NULL,
                                    amount DECIMAL(18,2) NOT NULL,
                                    balance_after DECIMAL(18,2) NOT NULL,
                                transaction_date DATETIME NOT NULL DEFAULT GETDATE(),
                                CONSTRAINT FK_transactions_accounts FOREIGN KEY (cardnum) REFERENCES accounts(cardnum)
                                )
                            END";
                        using (SqlCommand command = new SqlCommand(createTransactionsQuery, connection))
                        {
                            command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing database: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public AuthResult LoginUser(string cardnum, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(cardnum) || string.IsNullOrEmpty(password))
                {
                    return new AuthResult { Success = false, Message = "Card number and password are required" };
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT password FROM accounts WHERE cardnum = @cardnum";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        var result = command.ExecuteScalar();

                        if (result == null)
                        {
                            return new AuthResult { Success = false, Message = "Account not found" };
                        }

                        string storedPassword = result.ToString();
                        if (storedPassword == password)
                        {
                            return new AuthResult { Success = true, Message = "Login successful" };
                        }
                        else
                        {
                            return new AuthResult { Success = false, Message = "Invalid password" };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Error during login: {ex.Message}" };
            }
        }

        public AuthResult RegisterUser(string cardnum, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(cardnum) || string.IsNullOrEmpty(password))
                {
                    return new AuthResult { Success = false, Message = "Card number and password are required" };
                }
                
                using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM accounts WHERE cardnum = @cardnum";
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            return new AuthResult { Success = false, Message = "Card number already exists" };
                        }
                    }

                    string insertQuery = "INSERT INTO accounts (cardnum, password, balance) VALUES (@cardnum, @password, 0)";
                    using (var insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        insertCommand.Parameters.Add("@password", SqlDbType.NVarChar, 100).Value = password;
                        insertCommand.ExecuteNonQuery();
                    }

                    return new AuthResult { Success = true, Message = "Registration successful" };
                }
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Error during registration: {ex.Message}" };
            }
        }

        public bool VerifyCardNumber(string cardnum)
        {
            try
            {
                if (string.IsNullOrEmpty(cardnum))
                {
                    return false;
                }

                using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                    string query = "SELECT COUNT(*) FROM accounts WHERE cardnum = @cardnum";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
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
                        command.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        var result = command.ExecuteScalar();
                        return result != null ? Convert.ToDecimal(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving balance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        public List<Transaction> GetTransactionHistory(string cardnum)
        {
            List<Transaction> transactions = new List<Transaction>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id, cardnum, type, amount, balance_after, transaction_date " +
                                   "FROM transactions WHERE cardnum = @cardnum " +
                                   "ORDER BY transaction_date DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cardnum", cardnum);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new Transaction
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    CardNumber = reader["cardnum"].ToString(),
                                    Type = reader["type"].ToString(),
                                    Amount = Convert.ToDecimal(reader["amount"]),
                                    BalanceAfter = Convert.ToDecimal(reader["balance_after"]),
                                    Date = Convert.ToDateTime(reader["transaction_date"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting transaction history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return transactions;
        }

        public AuthResult UpdatePassword(string cardnum, string oldPassword, string newPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(cardnum) || string.IsNullOrEmpty(newPassword))
                {
                    return new AuthResult { Success = false, Message = "Card number and new password are required" };
                }
                
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM accounts WHERE cardnum = @cardnum";
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count == 0)
                        {
                            return new AuthResult { Success = false, Message = "Account not found" };
                        }
                    }

                    string updateQuery = "UPDATE accounts SET password = @newPassword WHERE cardnum = @cardnum";
                    using (var updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.Add("@newPassword", SqlDbType.NVarChar, 100).Value = newPassword;
                        updateCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        updateCommand.ExecuteNonQuery();
                    }

                    return new AuthResult { Success = true, Message = "Password updated successfully" };
                }
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Error updating password: {ex.Message}" };
            }
        }

        public bool Deposit(string cardnum, decimal amount)
        {
            try
            {
                if (string.IsNullOrEmpty(cardnum) || amount <= 0)
                {
                    return false;
                }
                
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string checkQuery = "SELECT balance FROM accounts WHERE cardnum = @cardnum";
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        var result = checkCommand.ExecuteScalar();

                        if (result == null)
                        {
                            return false;
                        }

                        decimal currentBalance = Convert.ToDecimal(result);
                        decimal depositAmount = amount / 2; // تقسيم المبلغ على 2 للإيداع
                        decimal newBalance = currentBalance + depositAmount;

                        string updateQuery = "UPDATE accounts SET balance = @newBalance WHERE cardnum = @cardnum";
                        using (var updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.Add("@newBalance", SqlDbType.Decimal).Value = newBalance;
                            updateCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                            updateCommand.ExecuteNonQuery();
                        }

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Withdraw(string cardnum, decimal amount)
        {
            try
            {
                if (string.IsNullOrEmpty(cardnum) || amount <= 0)
                {
                    return false;
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string checkQuery = "SELECT balance FROM accounts WHERE cardnum = @cardnum";
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        var result = checkCommand.ExecuteScalar();

                        if (result == null)
                        {
                            return false;
                        }

                        decimal currentBalance = Convert.ToDecimal(result);

                        if (amount > currentBalance)
                        {
                            return false;
                        }

                        decimal newBalance = currentBalance - amount;

                        string updateQuery = "UPDATE accounts SET balance = @newBalance WHERE cardnum = @cardnum";
                        using (var updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.Add("@newBalance", SqlDbType.Decimal).Value = newBalance;
                            updateCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                            updateCommand.ExecuteNonQuery();
                        }

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TransferMoney(string sourceCardNumber, string destinationCardNumber, decimal amount)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // التحقق من وجود الحساب المصدر
                            string checkSourceQuery = "SELECT COUNT(*) FROM accounts WHERE cardnum = @sourceCardNumber";
                            using (var checkSourceCommand = new SqlCommand(checkSourceQuery, connection, transaction))
                            {
                                checkSourceCommand.Parameters.Add("@sourceCardNumber", SqlDbType.NVarChar, 50).Value = sourceCardNumber;
                                int sourceCount = Convert.ToInt32(checkSourceCommand.ExecuteScalar());
                                if (sourceCount == 0)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            // التحقق من وجود الحساب الهدف
                            string checkDestinationQuery = "SELECT COUNT(*) FROM accounts WHERE cardnum = @destinationCardNumber";
                            using (var checkDestinationCommand = new SqlCommand(checkDestinationQuery, connection, transaction))
                            {
                                checkDestinationCommand.Parameters.Add("@destinationCardNumber", SqlDbType.NVarChar, 50).Value = destinationCardNumber;
                                int destinationCount = Convert.ToInt32(checkDestinationCommand.ExecuteScalar());
                                if (destinationCount == 0)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            // التحقق من رصيد الحساب المصدر
                            string checkBalanceQuery = "SELECT balance FROM accounts WHERE cardnum = @sourceCardNumber";
                            decimal sourceBalance;
                            using (var checkCommand = new SqlCommand(checkBalanceQuery, connection, transaction))
                            {
                                checkCommand.Parameters.Add("@sourceCardNumber", SqlDbType.NVarChar, 50).Value = sourceCardNumber;
                                sourceBalance = Convert.ToDecimal(checkCommand.ExecuteScalar());

                                if (amount > sourceBalance)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            // خصم المبلغ من الحساب المصدر
                            string withdrawQuery = "UPDATE accounts SET balance = balance - @amount WHERE cardnum = @sourceCardNumber";
                            using (var withdrawCommand = new SqlCommand(withdrawQuery, connection, transaction))
                            {
                                withdrawCommand.Parameters.Add("@amount", SqlDbType.Decimal).Value = amount;
                                withdrawCommand.Parameters.Add("@sourceCardNumber", SqlDbType.NVarChar, 50).Value = sourceCardNumber;
                                int rowsAffected = withdrawCommand.ExecuteNonQuery();
                                if (rowsAffected != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            // التحقق من الرصيد بعد الخصم
                            string verifySourceBalanceQuery = "SELECT balance FROM accounts WHERE cardnum = @sourceCardNumber";
                            using (var verifyCommand = new SqlCommand(verifySourceBalanceQuery, connection, transaction))
                            {
                                verifyCommand.Parameters.Add("@sourceCardNumber", SqlDbType.NVarChar, 50).Value = sourceCardNumber;
                                decimal newSourceBalance = Convert.ToDecimal(verifyCommand.ExecuteScalar());
                                if (newSourceBalance != sourceBalance - amount)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            // إضافة المبلغ للحساب الهدف
                            string depositQuery = "UPDATE accounts SET balance = balance + @amount WHERE cardnum = @destinationCardNumber";
                            using (var depositCommand = new SqlCommand(depositQuery, connection, transaction))
                            {
                                depositCommand.Parameters.Add("@amount", SqlDbType.Decimal).Value = amount;
                                depositCommand.Parameters.Add("@destinationCardNumber", SqlDbType.NVarChar, 50).Value = destinationCardNumber;
                                int rowsAffected = depositCommand.ExecuteNonQuery();
                                if (rowsAffected != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            // التحقق من الرصيد بعد الإضافة
                            string verifyDestinationBalanceQuery = "SELECT balance FROM accounts WHERE cardnum = @destinationCardNumber";
                            using (var verifyCommand = new SqlCommand(verifyDestinationBalanceQuery, connection, transaction))
                            {
                                verifyCommand.Parameters.Add("@destinationCardNumber", SqlDbType.NVarChar, 50).Value = destinationCardNumber;
                                decimal newDestinationBalance = Convert.ToDecimal(verifyCommand.ExecuteScalar());
                                if (newDestinationBalance <= 0)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}