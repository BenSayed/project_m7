using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace project_m7
{

    //massage
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

   







    //database init
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
                                firstname NVARCHAR(100) NOT NULL,
                                lastname NVARCHAR(100) NOT NULL,
                                password NVARCHAR(100) NOT NULL,
                                balance DECIMAL(18,2) NOT NULL DEFAULT 0,
                                is_enabled BIT NOT NULL DEFAULT 1,
                               

                            )
                        END
                        ELSE
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('accounts') AND name = 'firstname')
                            BEGIN
                                ALTER TABLE accounts ADD firstname NVARCHAR(100) NOT NULL DEFAULT ''
                            END
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('accounts') AND name = 'lastname')
                            BEGIN
                                ALTER TABLE accounts ADD lastname NVARCHAR(100) NOT NULL DEFAULT ''
                            END
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('accounts') AND name = 'is_enabled')
                            BEGIN
                                ALTER TABLE accounts ADD is_enabled BIT NOT NULL DEFAULT 1
                            END
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('accounts') AND name = 'phone_number')
                            BEGIN
                                ALTER TABLE accounts ADD phone_number NVARCHAR(11) NOT NULL DEFAULT ''
                            END
                        END";
                    using (SqlCommand command = new SqlCommand(createAccountsQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Create Transactions table if it doesn't exist
                    string createTransactionsQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'transactions')
                        BEGIN
                            CREATE TABLE transactions (
                                transaction_id INT IDENTITY(1,1) PRIMARY KEY,
                                cardnum NVARCHAR(50) NOT NULL,
                                transaction_type NVARCHAR(50) NOT NULL,
                                amount DECIMAL(18,2) NOT NULL,
                                transaction_date DATETIME NOT NULL DEFAULT GETDATE(),
                                balance_after DECIMAL(18,2) NOT NULL,
                                destination_cardnum NVARCHAR(50) NULL,
                                description NVARCHAR(255) NULL,
                                FOREIGN KEY (cardnum) REFERENCES accounts(cardnum)
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






        //Register
        public AuthResult RegisterUser(string cardnum, string firstname, string lastname, string password, string phone)
        {
            try
            {
                if (string.IsNullOrEmpty(cardnum) || string.IsNullOrEmpty(password) || 
                    string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname) || string.IsNullOrEmpty(phone))
                {
                    return new AuthResult { Success = false, Message = "All fields are required" };
                }
                
                // Validate phone number
                
                
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
                    
                    // Format the phone number to store only digits
                   
                    
                    string insertQuery = "INSERT INTO accounts (cardnum, firstname, lastname, password, balance, phone_number) VALUES (@cardnum, @firstname, @lastname, @password, 0, @phone)";
                    using (var insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        insertCommand.Parameters.Add("@firstname", SqlDbType.NVarChar, 100).Value = firstname;
                        insertCommand.Parameters.Add("@lastname", SqlDbType.NVarChar, 100).Value = lastname;
                        insertCommand.Parameters.Add("@password", SqlDbType.NVarChar, 100).Value = password;
                        insertCommand.Parameters.Add("@phone", SqlDbType.NVarChar, 11).Value = phone;
                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return new AuthResult { Success = true, Message = "Registration successful" };
                        }
                        else
                        {
                            return new AuthResult { Success = false, Message = "Failed to create account" };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Error during registration: {ex.Message}" };
            }
        }

      
        //card number check
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






        //get balance for deposit and withdral and transfare
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

       
        //for forgeting password
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





        //deposit process
        public bool Deposit(string cardNumber, decimal amount)
        {
            if (!IsCardEnabled(cardNumber))
            {
                MessageBox.Show("لا يمكنك إيداع الأموال لأن البطاقة معطلة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Get current balance before update
                    decimal currentBalance = 0;
                    string balanceQuery = "SELECT balance FROM accounts WHERE cardnum = @cardnum";
                    using (SqlCommand balanceCommand = new SqlCommand(balanceQuery, connection))
                    {
                        balanceCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                        var result = balanceCommand.ExecuteScalar();
                        currentBalance = result != null ? Convert.ToDecimal(result) : 0;
                    }
                    
                    // Update the balance
                    string updateQuery = "UPDATE accounts SET balance = balance + @amount WHERE cardnum = @cardnum";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@amount", amount);
                        updateCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            // Record the transaction
                            decimal newBalance = currentBalance + amount;
                            RecordTransaction(connection, cardNumber, "Deposit", amount, newBalance, null, "Cash deposit");
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //Withdrawal process
        public bool Withdraw(string cardNumber, decimal amount)
        {
            if (!IsCardEnabled(cardNumber))
            {
                MessageBox.Show("لا يمكنك سحب الأموال لأن البطاقة معطلة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Check if there's enough balance
                    string query = "SELECT balance FROM accounts WHERE cardnum = @cardnum";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cardnum", cardNumber);
                        var result = command.ExecuteScalar();
                        
                        if (result == null)
                        {
                            MessageBox.Show("لم يتم العثور على الحساب", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        
                        decimal currentBalance = Convert.ToDecimal(result);
                        
                        if (currentBalance < amount)
                        {
                            MessageBox.Show("رصيدك غير كافٍ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        // Perform the withdrawal
                        string updateQuery = "UPDATE accounts SET balance = balance - @amount WHERE cardnum = @cardnum";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@amount", amount);
                            updateCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                            int rowsAffected = updateCommand.ExecuteNonQuery();
                            
                            if (rowsAffected > 0)
                            {
                                // Calculate new balance
                                decimal newBalance = currentBalance - amount;
                                
                                // Record the transaction
                                RecordTransaction(connection, cardNumber, "Withdrawal", amount, newBalance, null, "Cash withdrawal");
                                return true;
                            }
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }



       // transfer process
        public bool TransferMoney(string sourceCardNumber, string destinationCardNumber, decimal amount)
        {
            if (!IsCardEnabled(sourceCardNumber))
            {
                MessageBox.Show("لا يمكنك تحويل الأموال لأن البطاقة معطلة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Check source account balance
                            decimal sourceCurrentBalance = 0;
                            string checkBalanceQuery = "SELECT balance FROM accounts WHERE cardnum = @cardnum";
                            using (SqlCommand command = new SqlCommand(checkBalanceQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@cardnum", sourceCardNumber);
                                var result = command.ExecuteScalar();
                                sourceCurrentBalance = result != null ? Convert.ToDecimal(result) : 0;

                                if (sourceCurrentBalance < amount)
                                {
                                    MessageBox.Show("رصيدك غير كافٍ", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            // Get destination account balance before update
                            decimal destCurrentBalance = 0;
                            using (SqlCommand command = new SqlCommand(checkBalanceQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@cardnum", destinationCardNumber);
                                var result = command.ExecuteScalar();
                                destCurrentBalance = result != null ? Convert.ToDecimal(result) : 0;
                            }

                            // Deduct from source account
                            string deductQuery = "UPDATE accounts SET balance = balance - @amount WHERE cardnum = @cardnum";
                            using (SqlCommand command = new SqlCommand(deductQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@amount", amount);
                                command.Parameters.AddWithValue("@cardnum", sourceCardNumber);
                                command.ExecuteNonQuery();
                            }

                            // Add to destination account
                            string addQuery = "UPDATE accounts SET balance = balance + @amount WHERE cardnum = @cardnum";
                            using (SqlCommand command = new SqlCommand(addQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@amount", amount);
                                command.Parameters.AddWithValue("@cardnum", destinationCardNumber);
                                command.ExecuteNonQuery();
                            }

                            // Record transaction for source account (money sent)
                            decimal sourceNewBalance = sourceCurrentBalance - amount;
                            string recordSourceQuery = @"
                                INSERT INTO transactions (cardnum, transaction_type, amount, transaction_date, balance_after, destination_cardnum, description)
                                VALUES (@cardnum, @transactionType, @amount, GETDATE(), @balanceAfter, @destinationCardnum, @description)
                            ";
                            using (SqlCommand command = new SqlCommand(recordSourceQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@cardnum", sourceCardNumber);
                                command.Parameters.AddWithValue("@transactionType", "Transfer Out");
                                command.Parameters.AddWithValue("@amount", amount);
                                command.Parameters.AddWithValue("@balanceAfter", sourceNewBalance);
                                command.Parameters.AddWithValue("@destinationCardnum", destinationCardNumber);
                                command.Parameters.AddWithValue("@description", "Money transfer to another account");
                                command.ExecuteNonQuery();
                            }

                            // Record transaction for destination account (money received)
                            decimal destNewBalance = destCurrentBalance + amount;
                            string recordDestQuery = @"
                                INSERT INTO transactions (cardnum, transaction_type, amount, transaction_date, balance_after, destination_cardnum, description)
                                VALUES (@cardnum, @transactionType, @amount, GETDATE(), @balanceAfter, @destinationCardnum, @description)
                            ";
                            using (SqlCommand command = new SqlCommand(recordDestQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@cardnum", destinationCardNumber);
                                command.Parameters.AddWithValue("@transactionType", "Transfer In");
                                command.Parameters.AddWithValue("@amount", amount);
                                command.Parameters.AddWithValue("@balanceAfter", destNewBalance);
                                command.Parameters.AddWithValue("@destinationCardnum", sourceCardNumber);
                                command.Parameters.AddWithValue("@description", "Money received from another account");
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }



        //password check
        public bool VerifyPassword(string cardNumber, string password)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM accounts WHERE cardnum = @cardnum AND password = @password";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardNumber;
                        command.Parameters.Add("@password", SqlDbType.NVarChar, 100).Value = password;
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






        //delete account
        public bool DeleteAccount(string cardNumber)
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
                            // حذف جميع المعاملات المرتبطة بالحساب
                            string deleteTransactionsQuery = "DELETE FROM transactions WHERE cardnum = @cardnum";
                            using (var command = new SqlCommand(deleteTransactionsQuery, connection, transaction))
                            {
                                command.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardNumber;
                                command.ExecuteNonQuery();
                            }

                            // حذف الحساب
                            string deleteAccountQuery = "DELETE FROM accounts WHERE cardnum = @cardnum";
                            using (var command = new SqlCommand(deleteAccountQuery, connection, transaction))
                            {
                                command.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardNumber;
                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    transaction.Commit();
                                    return true;
                                }
                            }
                            transaction.Rollback();
                            return false;
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

        
        
        
        
        //disable card
        public bool DisableCard(string cardNumber)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE accounts SET is_enabled = 0 WHERE cardnum = @cardnum";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardNumber;
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }






        //check in settings
        public bool ToggleCardStatus(string cardNumber, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // First verify the password
                    string verifyQuery = "SELECT COUNT(*) FROM accounts WHERE cardnum = @cardnum AND password = @password";
                    using (SqlCommand verifyCommand = new SqlCommand(verifyQuery, connection))
                    {
                        verifyCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                        verifyCommand.Parameters.AddWithValue("@password", password);
                        int count = Convert.ToInt32(verifyCommand.ExecuteScalar());
                        
                        if (count == 0)
                        {
                            MessageBox.Show("Incorrect password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                    // Get current status (0 = disabled, 1 = enabled)
                    string statusQuery = "SELECT is_enabled FROM accounts WHERE cardnum = @cardnum";
                    int currentStatus;
                    using (SqlCommand statusCommand = new SqlCommand(statusQuery, connection))
                    {
                        statusCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                        currentStatus = Convert.ToInt32(statusCommand.ExecuteScalar());
                    }

                    // Toggle the status
                    string updateQuery = "UPDATE accounts SET is_enabled = @newStatus WHERE cardnum = @cardnum";
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        int newStatus = currentStatus == 1 ? 0 : 1; // Toggle between 0 and 1
                        updateCommand.Parameters.AddWithValue("@newStatus", newStatus);
                        updateCommand.Parameters.AddWithValue("@cardnum", cardNumber);
                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            string message = currentStatus == 1 ? 
                                "Card has been disabled successfully" : 
                                "Card has been enabled successfully";
                            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }



        //check the card is active
        public bool IsCardEnabled(string cardNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT is_enabled FROM accounts WHERE cardnum = @cardnum";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cardnum", cardNumber);
                    object result = command.ExecuteScalar();
                    return result != null && Convert.ToBoolean(result);
                }
            }
        }








        //Donate process
        // Helper method to record transactions
        private bool RecordTransaction(SqlConnection connection, string cardnum, string transactionType, decimal amount, decimal balanceAfter, string destinationCardnum = null, string description = null)
        {
            try
            {
                string query = @"
                    INSERT INTO transactions (cardnum, transaction_type, amount, transaction_date, balance_after, destination_cardnum, description)
                    VALUES (@cardnum, @transactionType, @amount, GETDATE(), @balanceAfter, @destinationCardnum, @description)
                ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@cardnum", cardnum);
                    command.Parameters.AddWithValue("@transactionType", transactionType);
                    command.Parameters.AddWithValue("@amount", amount);
                    command.Parameters.AddWithValue("@balanceAfter", balanceAfter);
                    command.Parameters.AddWithValue("@destinationCardnum", destinationCardnum ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@description", description ?? (object)DBNull.Value);

                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error recording transaction: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public AuthResult Donate(string cardnum, string password, decimal amount)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(cardnum) || string.IsNullOrEmpty(password) || amount <= 0)
                {
                    return new AuthResult { Success = false, Message = "Invalid donation information" };
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Verify the password
                    string verifyQuery = "SELECT COUNT(*) FROM accounts WHERE cardnum = @cardnum AND password = @password";
                    using (SqlCommand verifyCommand = new SqlCommand(verifyQuery, connection))
                    {
                        verifyCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        verifyCommand.Parameters.Add("@password", SqlDbType.NVarChar, 100).Value = password;
                        int count = Convert.ToInt32(verifyCommand.ExecuteScalar());

                        if (count == 0)
                        {
                            return new AuthResult { Success = false, Message = "Invalid card number or password" };
                        }
                    }

                    // Check if the account has sufficient balance
                    string balanceQuery = "SELECT balance FROM accounts WHERE cardnum = @cardnum";
                    decimal currentBalance = 0;

                    using (SqlCommand balanceCommand = new SqlCommand(balanceQuery, connection))
                    {
                        balanceCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        var result = balanceCommand.ExecuteScalar();
                        currentBalance = result != null ? Convert.ToDecimal(result) : 0;

                        if (currentBalance < amount)
                        {
                            return new AuthResult { Success = false, Message = "Insufficient balance for donation" };
                        }
                    }

                    // Update the balance
                    decimal newBalance = currentBalance - amount;
                    string updateQuery = "UPDATE accounts SET balance = @newBalance WHERE cardnum = @cardnum";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.Add("@newBalance", SqlDbType.Decimal).Value = newBalance;
                        updateCommand.Parameters.Add("@cardnum", SqlDbType.NVarChar, 50).Value = cardnum;
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            return new AuthResult { Success = false, Message = "Failed to process donation" };
                        }
                    }

                    // Record the transaction
                    RecordTransaction(connection, cardnum, "Donation", amount, newBalance, null, "Charitable donation");

                    return new AuthResult
                    {
                        Success = true,
                        Message = $"Thank you for your donation of {amount:C}! Your new balance is {newBalance:C}."
                    };
                }
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Error processing donation: {ex.Message}" };
            }
        }




    }
}

