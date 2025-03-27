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
    
    public async Task<int> InsertDoctorAndPatientByUserLogin(string login, string doctorName, string patientName)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("USE Hospital");
        int result = await connection.ExecuteAsync(DbCommands.InsertDoctorAndPatientByUserLogin(login, doctorName, patientName));
        return result;
    }

    public async Task<IEnumerable<dynamic>> GetPatientsAndDoctorByUserLogin(string login)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("USE Hospital");
        // TO_DO: Refactor this (if no DB - has error)
        var result = await connection.QueryAsync(DbCommands.GetPatientsByDoctorLogin(login));
        var enumerable = result.ToList();
        if (enumerable.ToList().Count == 0)
        {
            return null;
        }

        return enumerable;
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