
using Npgsql;

namespace ExamMVC;

public class DepartmentRepository : IDepartmentInterface
{
    private readonly NpgsqlConnection _connection;

    public DepartmentRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    public async Task<List<dynamic>> GetAll()
    {
        List<dynamic> departments = new List<dynamic>();

        await _connection.OpenAsync();

        string query = @"select * from t_department order by c_departmentid";

        using NpgsqlCommand command = new NpgsqlCommand(query,_connection);

        using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        while(await reader.ReadAsync())
        {
            departments.Add(new
            {
                c_departmentid = Convert.ToInt32(reader["c_departmentid"]),
                c_departmentname = reader["c_departmentname"].ToString()!
            });
        }
        await _connection.CloseAsync();
        return departments;
    }
}
