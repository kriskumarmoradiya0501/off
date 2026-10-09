using examfinal.Models;
using Npgsql;

namespace examfinal.Repositories;

public class ExamRepository
{
    private readonly NpgsqlConnection _connection;

    public ExamRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<bool> RegisterAdmin(AdminModel admin)
    {
        await _connection.OpenAsync();

        try
        {
            const string checkQuery = @"
                SELECT EXISTS (
                    SELECT 1
                    FROM t_admin6
                    WHERE c_email = @c_email
                )";

            await using var checkCommand = new NpgsqlCommand(checkQuery, _connection);
            checkCommand.Parameters.AddWithValue("@c_email", admin.c_email);

            var emailExists = (bool)(await checkCommand.ExecuteScalarAsync() ?? false);
            if (emailExists)
            {
                return false;
            }

            const string insertQuery = @"
                INSERT INTO t_admin6 (c_email, c_password)
                VALUES (@c_email, @c_password)";

            await using var insertCommand = new NpgsqlCommand(insertQuery, _connection);
            insertCommand.Parameters.AddWithValue("@c_email", admin.c_email);
            insertCommand.Parameters.AddWithValue("@c_password", admin.c_password);

            return await insertCommand.ExecuteNonQueryAsync() > 0;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<AdminModel?> LoginAdmin(string email, string password)
    {
        await _connection.OpenAsync();

        try
        {
            const string query = @"
                SELECT c_adminid, c_email
                FROM t_admin6
                WHERE c_email = @c_email
                AND c_password = @c_password";

            await using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("@c_email", email);
            command.Parameters.AddWithValue("@c_password", password);
            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return new AdminModel
            {
                c_adminid = Convert.ToInt32(reader["c_adminid"]),
                c_email = reader["c_email"].ToString()!
            };
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<DepartmentModel>> GetDepartments()
    {
        var departments = new List<DepartmentModel>();
        await _connection.OpenAsync();

        try
        {
            const string query = @"
                SELECT c_departmentid, c_departmentname
                FROM t_department6
                ORDER BY c_departmentname";

            await using var command = new NpgsqlCommand(query, _connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                departments.Add(new DepartmentModel
                {
                    c_departmentid = Convert.ToInt32(reader["c_departmentid"]),
                    c_departmentname = reader["c_departmentname"].ToString()!
                });
            }

            return departments;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<UserModel>> GetUsers(int adminId)
    {
        var users = new List<UserModel>();
        await _connection.OpenAsync();

        try
        {
            const string query = @"
                SELECT u.c_userid, u.c_name, u.c_email, u.c_mobile,
                       u.c_gender, u.c_hobbies, u.c_departmentid,
                       u.c_dob, u.c_age, u.c_address, u.c_image, u.c_status,
                       d.c_departmentname
                FROM t_user6 u
                JOIN t_department6 d ON d.c_departmentid = u.c_departmentid
                WHERE u.c_adminid = @c_adminid
                ORDER BY u.c_userid";

            await using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("@c_adminid", adminId);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new UserModel
                {
                    c_userid = Convert.ToInt32(reader["c_userid"]),
                    c_name = reader["c_name"].ToString()!,
                    c_email = reader["c_email"].ToString()!,
                    c_mobile = reader["c_mobile"].ToString()!,
                    c_gender = reader["c_gender"].ToString()!,
                    c_hobbies = reader["c_hobbies"] == DBNull.Value
                        ? null
                        : reader["c_hobbies"].ToString(),
                    c_departmentid = Convert.ToInt32(reader["c_departmentid"]),
                    c_dob = (DateOnly)reader["c_dob"],
                    c_age = reader["c_age"] == DBNull.Value
                        ? null
                        : Convert.ToInt32(reader["c_age"]),
                    c_address = reader["c_address"] == DBNull.Value
                        ? null
                        : reader["c_address"].ToString(),
                    c_image = reader["c_image"] == DBNull.Value
                        ? null
                        : reader["c_image"].ToString(),
                    c_status = Convert.ToBoolean(reader["c_status"]),
                    c_departmentname = reader["c_departmentname"].ToString()!
                });
            }

            return users;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<UserModel?> GetUser(int id, int adminId)
    {
        await _connection.OpenAsync();

        try
        {
            const string query = @"
                SELECT u.c_userid, u.c_name, u.c_email, u.c_mobile,
                       u.c_gender, u.c_hobbies, u.c_departmentid,
                       u.c_dob, u.c_age, u.c_address, u.c_image, u.c_status,
                       d.c_departmentname
                FROM t_user6 u
                JOIN t_department6 d ON d.c_departmentid = u.c_departmentid
                WHERE u.c_userid = @c_userid AND u.c_adminid = @c_adminid";

            await using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("@c_userid", id);
            command.Parameters.AddWithValue("@c_adminid", adminId);
            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            var user = new UserModel
            {
                c_userid = Convert.ToInt32(reader["c_userid"]),
                c_name = reader["c_name"].ToString()!,
                c_email = reader["c_email"].ToString()!,
                c_mobile = reader["c_mobile"].ToString()!,
                c_gender = reader["c_gender"].ToString()!,
                c_hobbies = reader["c_hobbies"] == DBNull.Value
                    ? null
                    : reader["c_hobbies"].ToString(),
                c_departmentid = Convert.ToInt32(reader["c_departmentid"]),
                c_dob = (DateOnly)reader["c_dob"],
                c_age = reader["c_age"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(reader["c_age"]),
                c_address = reader["c_address"] == DBNull.Value
                    ? null
                    : reader["c_address"].ToString(),
                c_image = reader["c_image"] == DBNull.Value
                    ? null
                    : reader["c_image"].ToString(),
                c_status = Convert.ToBoolean(reader["c_status"]),
                c_departmentname = reader["c_departmentname"].ToString()!
            };

            return user;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> AddUser(UserModel user, int adminId)
    {
        await _connection.OpenAsync();

        try
        {
            const string query = @"
                INSERT INTO t_user6
                (c_adminid, c_name, c_email, c_mobile, c_gender, c_hobbies,
                 c_departmentid, c_dob, c_age, c_address, c_image, c_status)
                VALUES
                (@c_adminid, @c_name, @c_email, @c_mobile, @c_gender, @c_hobbies,
                 @c_departmentid, @c_dob, @c_age, @c_address, @c_image, @c_status)";

            await using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("@c_adminid", adminId);
            command.Parameters.AddWithValue("@c_name", user.c_name);
            command.Parameters.AddWithValue("@c_email", user.c_email);
            command.Parameters.AddWithValue("@c_mobile", user.c_mobile);
            command.Parameters.AddWithValue("@c_gender", user.c_gender);
            command.Parameters.AddWithValue("@c_hobbies", (object?)user.c_hobbies ?? DBNull.Value);
            command.Parameters.AddWithValue("@c_departmentid", user.c_departmentid);
            command.Parameters.AddWithValue("@c_dob", user.c_dob!.Value);
            command.Parameters.AddWithValue("@c_age", (object?)user.c_age ?? DBNull.Value);
            command.Parameters.AddWithValue("@c_address", (object?)user.c_address ?? DBNull.Value);
            command.Parameters.AddWithValue("@c_image", (object?)user.c_image ?? DBNull.Value);
            command.Parameters.AddWithValue("@c_status", user.c_status);

            return await command.ExecuteNonQueryAsync();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> UpdateUser(UserModel user, int adminId)
    {
        await _connection.OpenAsync();

        try
        {
            const string query = @"
                UPDATE t_user6
                SET c_name = @c_name,
                    c_email = @c_email,
                    c_mobile = @c_mobile,
                    c_gender = @c_gender,
                    c_hobbies = @c_hobbies,
                    c_departmentid = @c_departmentid,
                    c_dob = @c_dob,
                    c_age = @c_age,
                    c_address = @c_address,
                    c_image = @c_image,
                    c_status = @c_status
                WHERE c_userid = @c_userid AND c_adminid = @c_adminid";

            await using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("@c_userid", user.c_userid);
            command.Parameters.AddWithValue("@c_adminid", adminId);
            command.Parameters.AddWithValue("@c_name", user.c_name);
            command.Parameters.AddWithValue("@c_email", user.c_email);
            command.Parameters.AddWithValue("@c_mobile", user.c_mobile);
            command.Parameters.AddWithValue("@c_gender", user.c_gender);
            command.Parameters.AddWithValue("@c_hobbies", (object?)user.c_hobbies ?? DBNull.Value);
            command.Parameters.AddWithValue("@c_departmentid", user.c_departmentid);
            command.Parameters.AddWithValue("@c_dob", user.c_dob!.Value);
            command.Parameters.AddWithValue("@c_age", (object?)user.c_age ?? DBNull.Value);
            command.Parameters.AddWithValue("@c_address", (object?)user.c_address ?? DBNull.Value);
            command.Parameters.AddWithValue("@c_image", (object?)user.c_image ?? DBNull.Value);
            command.Parameters.AddWithValue("@c_status", user.c_status);

            return await command.ExecuteNonQueryAsync();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> DeleteUser(int id, int adminId)
    {
        await _connection.OpenAsync();

        try
        {
            const string query = @"
                DELETE FROM t_user6
                WHERE c_userid = @c_userid AND c_adminid = @c_adminid";

            await using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("@c_userid", id);
            command.Parameters.AddWithValue("@c_adminid", adminId);

            return await command.ExecuteNonQueryAsync();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

}
