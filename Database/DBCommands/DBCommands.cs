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
                                UserId INT FOREIGN KEY REFERENCES Users(Id)
                            );
                        END
                    
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Patients]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE Patients (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                Name NVARCHAR(100) NOT NULL
                            );
                        END
                    
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Doctors]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE Doctors (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                Name NVARCHAR(100) NOT NULL,
                                DepartmentId INT FOREIGN KEY REFERENCES Departments(Id)
                            );
                        END
                    
                        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DoctorsAndPatients]') AND type in (N'U'))
                        BEGIN
                            CREATE TABLE DoctorsAndPatients (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                DoctorId INT FOREIGN KEY REFERENCES Doctors(Id),
                                PatientId INT FOREIGN KEY REFERENCES Patients(Id)
                            );
                        END";

    public static string DropTablesCommand() => @"
                    DROP TABLE IF EXISTS Doctors;
                    DROP TABLE IF EXISTS DoctorsAndPatients;
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

   public static string InsertDoctorAndPatientByUserLogin(string login, string doctorName, string patientName)
   {
       return $@"USE Hospital;
                   DECLARE @userId INT;
                   SELECT @userId = Id FROM Users WHERE Login = '{login}';
   
                   DECLARE @departmentId INT;
                   SELECT @departmentId = Id FROM Departments WHERE UserId = @userId;
                   IF @departmentId IS NULL
                   BEGIN
                       INSERT INTO Departments (Name, UserId) VALUES ('Default Department', @userId);
                       SELECT @departmentId = SCOPE_IDENTITY();
                   END
   
                   DECLARE @doctorId INT;
                   SELECT @doctorId = Id FROM Doctors WHERE DepartmentId = @departmentId;
                   IF @doctorId IS NULL
                   BEGIN
                       INSERT INTO Doctors (Name, DepartmentId) VALUES ('{doctorName}', @departmentId);
                       SELECT @doctorId = SCOPE_IDENTITY();
                   END
   
                   DECLARE @patientId INT;
                   SELECT @patientId = Id FROM Patients WHERE Name = '{patientName}';
                   IF @patientId IS NULL
                   BEGIN
                       INSERT INTO Patients (Name) VALUES ('{patientName}');
                       SELECT @patientId = SCOPE_IDENTITY();
                   END
   
                   INSERT INTO DoctorsAndPatients (DoctorId, PatientId) VALUES (@doctorId, @patientId);";
   }

    public static string GetPatientsByDoctorLogin(string login)
    {
        return $@"
             USE Hospital;
             DECLARE @userId INT;
             SELECT @userId = Id FROM Users WHERE Login = '{login}';
             DECLARE @departmentId INT;
             SELECT @departmentId = Id FROM Departments WHERE UserId = @userId;
             DECLARE @doctorId INT;
             DECLARE @doctorName NVARCHAR(100);
             SELECT @doctorId = Id, @doctorName = Name FROM Doctors WHERE DepartmentId = @departmentId;
             SELECT
                 @doctorId AS DoctorId,
                 @doctorName AS DoctorName,
                 p.Id AS PatientId,
                 p.Name AS PatientName
             FROM
                 Patients p
             INNER JOIN
                 DoctorsAndPatients dp ON p.Id = dp.PatientId
             WHERE
                 dp.DoctorId = @doctorId;";
    }
}