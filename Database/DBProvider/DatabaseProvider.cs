using System.Data.SqlClient;
using Dapper;
using Database.DBCommands;
using Database.Models;

namespace Database.DBProvider;

public class DatabaseProvider
{
    private readonly string _connectionString;
        
    public DatabaseProvider(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<int> InsertDoctorAndPatientByUserLogin(string login, string patientName)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("USE Hospital");
        return await connection.ExecuteAsync(DbCommands.InsertDoctorAndPatientByUserLogin(login, patientName));
    }

    public async Task<List<PatientModel>> GetPatientsByDoctorLogin(string login)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("USE Hospital");
        var result = await connection.QueryAsync<PatientModel>(DbCommands.GetPatientsByDoctorLogin(login));
        return result.ToList();
    }
        
    [Obsolete("Obsolete")]
    public async Task<int> CreateUserAsync(UserModel user)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.ExecuteScalarAsync<int>(DbCommands.InsertUserCommand(user.login, user.password));
    }
        
    [Obsolete("Obsolete")]
    public async Task<UserModel?> GetUserAsync(string userLogin)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("USE Hospital");
        var user = await connection.QuerySingleOrDefaultAsync<UserModel>("SELECT * FROM dbo.Users WHERE Login = @login", new { login = userLogin } );
        
        return user;
    }
    
    [Obsolete("Obsolete")]
    public async Task InitializeDatabaseAsync()
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await connection.ExecuteAsync(DbCommands.CreateDbCommandWithNotExists("Hospital"));
            await connection.ExecuteAsync(DbCommands.CreateTablesCommandIfNotExist());
            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                CREATE TABLE Users (
                    id INT PRIMARY KEY IDENTITY,
                    login NVARCHAR(50) NOT NULL,
                    password NVARCHAR(50) NOT NULL
                )");
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            throw new Exception("Database initialization failed");
        }
    }
        
    [Obsolete("Obsolete")]
    public async Task ResetDatabaseAsync()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(DbCommands.DropTablesCommand());
        await connection.ExecuteAsync(DbCommands.CreateTablesCommandIfNotExist());
    }
}