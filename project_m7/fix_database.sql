-- This script will check and fix the database issues with the card number

-- First, check if the database exists
IF DB_ID('Bank_app') IS NULL
BEGIN
    PRINT 'Bank_app database does not exist. Creating it...'
    CREATE DATABASE Bank_app;
END
GO

USE Bank_app;
GO

-- Check if accounts table exists
IF OBJECT_ID('accounts', 'U') IS NULL
BEGIN
    PRINT 'accounts table does not exist. Creating it...'
    CREATE TABLE [accounts] (
        [cardnum] NVARCHAR(50) PRIMARY KEY,
        [password] NVARCHAR(100) NOT NULL,
        [balance] DECIMAL(18,2) NOT NULL DEFAULT 0
    );
END
GO

-- Drop foreign key constraints if they exist on transactions table
IF OBJECT_ID('transactions', 'U') IS NOT NULL
BEGIN
    DECLARE @sql NVARCHAR(MAX) = ''
    
    SELECT @sql = @sql + 'ALTER TABLE transactions DROP CONSTRAINT ' + name + ';'
    FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID('transactions')
    
    IF LEN(@sql) > 0
    BEGIN
        PRINT 'Dropping foreign key constraints on transactions table...'
        EXEC sp_executesql @sql
    END
END
GO

-- Check if transactions table exists, if not create it
IF OBJECT_ID('transactions', 'U') IS NULL
BEGIN
    PRINT 'transactions table does not exist. Creating it...'
    CREATE TABLE [transactions] (
        [id] INT IDENTITY(1,1) PRIMARY KEY,
        [cardnum] NVARCHAR(50) NOT NULL,
        [type] NVARCHAR(50) NOT NULL,
        [amount] DECIMAL(18,2) NOT NULL,
        [balance_after] DECIMAL(18,2) NOT NULL,
        [transaction_date] DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

-- Add foreign key constraint if it doesn't exist
IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID('transactions')
    AND referenced_object_id = OBJECT_ID('accounts')
)
BEGIN
    PRINT 'Adding foreign key constraint to transactions table...'
    ALTER TABLE [transactions]
    ADD CONSTRAINT FK_transactions_accounts FOREIGN KEY ([cardnum]) 
    REFERENCES [accounts]([cardnum]);
END
GO

-- Display existing accounts for debugging
PRINT 'Existing accounts:'
SELECT * FROM accounts;
GO

-- Check if the card number already exists
IF NOT EXISTS (SELECT 1 FROM accounts WHERE cardnum = '1111222233334444')
BEGIN
    PRINT 'The card number 1111222233334444 does not exist. Adding it...'
    INSERT INTO accounts (cardnum, password, balance)
    VALUES ('1111222233334444', 'secure123', 500.25);
    
    -- Add a transaction record for the account creation
    INSERT INTO transactions (cardnum, type, amount, balance_after, transaction_date)
    VALUES ('1111222233334444', 'Account Creation', 500.25, 500.25, GETDATE());
END
ELSE
BEGIN
    -- Make sure the password is correct
    UPDATE accounts
    SET password = 'secure123', balance = 500.25
    WHERE cardnum = '1111222233334444';
    
    PRINT 'The card number 1111222233334444 exists. Password has been updated to secure123';
END
GO

-- Add more test accounts
IF NOT EXISTS (SELECT 1 FROM accounts WHERE cardnum = '1234567890123456')
BEGIN
    PRINT 'Adding test account 1234567890123456...'
    INSERT INTO accounts (cardnum, password, balance)
    VALUES ('1234567890123456', 'pass123', 1000.00);
    
    INSERT INTO transactions (cardnum, type, amount, balance_after, transaction_date)
    VALUES ('1234567890123456', 'Account Creation', 1000.00, 1000.00, GETDATE());
END
GO

-- Display all accounts to verify
PRINT 'Current accounts after update:'
SELECT * FROM accounts;
GO

-- Remove any whitespace from cardnum values
UPDATE accounts
SET cardnum = LTRIM(RTRIM(cardnum))
WHERE cardnum <> LTRIM(RTRIM(cardnum));
GO

-- Display all transactions
PRINT 'Current transactions:'
SELECT * FROM transactions;
GO 