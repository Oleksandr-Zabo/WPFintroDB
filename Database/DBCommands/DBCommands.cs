namespace Database.DBCommands;
            
            public class DbCommands
            {
                public static string CreateDbCommandWithNotExists(string dbName) =>
                    $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{dbName}') CREATE DATABASE {dbName};";
            
                public static string UseDbCommand(string dbName) => $"USE {dbName};";
            
                public static string CreateTablesCommandIfNotExist() => @"
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE Users (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            Login NVARCHAR(100) NOT NULL UNIQUE,
                            Password NVARCHAR(100) NOT NULL
                        );
                    END
            
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Departments]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE Departments (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            Name NVARCHAR(100) NOT NULL,
                            CreatedAt DATETIME NOT NULL,
                            UserId INT FOREIGN KEY REFERENCES Users(Id)
                        );
                    END
            
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Patients]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE Patients (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            Name NVARCHAR(100) NOT NULL,
                            Count INT NULL
                        );
                    END
            
                    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Doctors]') AND type in (N'U'))
                    BEGIN
                        CREATE TABLE Doctors (
                            Id INT PRIMARY KEY IDENTITY(1,1),
                            Name NVARCHAR(100) NOT NULL,
                            PatientId INT FOREIGN KEY REFERENCES Patients(Id)
                        );
                    END";
            
                public static string DropTablesCommand() => @"
                    DROP TABLE IF EXISTS Doctors;
                    DROP TABLE IF EXISTS Patients;
                    DROP TABLE IF EXISTS Departments;
                    DROP TABLE IF EXISTS Users;";
            
                public static string InsertUserCommand(string login, string password)
                {
                    return $"USE Hospital; INSERT INTO Users (Login, Password) VALUES ('{login}', '{password}');";
                }
            
                public static string GetUserByLoginCommand()
                {
                    return "USE Hospital; SELECT * FROM Users WHERE Login = @login;";
                }
            }