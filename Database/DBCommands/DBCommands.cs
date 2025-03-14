namespace Database.DBCommands;
    
    public class DbCommands
    {
        public static string CreateDbCommandWithNotExists(string dbName) =>
            $"CREATE DATABASE IF NOT EXISTS {dbName};";
    
        public static string CreateTablesCommand() => @"
            CREATE TABLE Users (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Login NVARCHAR(100) NOT NULL UNIQUE,
                Password NVARCHAR(100) NOT NULL
            );

            CREATE TABLE Departments (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(100) NOT NULL,
                CreatedAt DATETIME NOT NULL,
                UserId INT FOREIGN KEY REFERENCES Users(Id)
            );
    
            CREATE TABLE Patients (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(100) NOT NULL,
                Count INT NULL
            );
    
            CREATE TABLE Doctors (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(100) NOT NULL,
                PatientId INT FOREIGN KEY REFERENCES Patients(Id)
            );
";
    
        public static string DropTablesCommand() => @"
            DROP TABLE IF EXISTS Departments;
            DROP TABLE IF EXISTS Doctors;
            DROP TABLE IF EXISTS Patients;
            DROP TABLE IF EXISTS Users;";
    
        public static string InsertUserCommand(string login, string password)
        {
            return $"INSERT INTO Users (Login, Password) VALUES ('{login}', '{password}');";
        }
    
        public static string GetUserCommand => "SELECT * FROM Users WHERE Login = @login;";
    }