using ExamMVC;
using Npgsql;

namespace ExamMVC;

public class UserRepository : IUserInterface
{
    private readonly NpgsqlConnection _connection;

    public UserRepository(NpgsqlConnection connection)
    {
        _connection = connection;
    }


    // =====================================================
    // GET ALL USERS
    // =====================================================

    public async Task<List<t_User4>> GetAll()
    {
        List<t_User4> users = new List<t_User4>();

        await _connection.OpenAsync();

        string query = @"
            SELECT *
            FROM t_user4
            ORDER BY c_userid;
        ";

        using NpgsqlCommand command =
            new NpgsqlCommand(query, _connection);

        using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var user = new t_User4
            {
                c_userid =
                    Convert.ToInt32(reader["c_userid"]),

                c_name =
                    reader["c_name"].ToString()!,

                c_email =
                    reader["c_email"].ToString()!,

                c_mobile =
                    reader["c_mobile"] == DBNull.Value
                        ? ""
                        : reader["c_mobile"].ToString(),

                c_password =
                    reader["c_password"].ToString()!,

                c_gender =
                    reader["c_gender"] == DBNull.Value
                        ? ""
                        : reader["c_gender"].ToString(),

                c_hobbies =
                    reader["c_hobbies"] == DBNull.Value
                        ? ""
                        : reader["c_hobbies"].ToString(),

                c_departmentid =
                    reader["c_departmentid"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            reader["c_departmentid"]
                        ),

                c_dob = (DateOnly)(reader["c_dob"]),
                c_age =
                    reader["c_age"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            reader["c_age"]
                        ),

                c_address =
                    reader["c_address"] == DBNull.Value
                        ? ""
                        : reader["c_address"].ToString(),

                c_image =
                    reader["c_image"] == DBNull.Value
                        ? ""
                        : reader["c_image"].ToString(),

                c_status =
                    reader["c_status"] == DBNull.Value
                        ? false
                        : Convert.ToBoolean(
                            reader["c_status"]
                        )
            };

            users.Add(user);
        }

        await _connection.CloseAsync();

        return users;
    }


    // =====================================================
    // GET USER BY ID
    // =====================================================

    public async Task<t_User4?> GetById(int id)
    {
        await _connection.OpenAsync();

        string query = @"
            SELECT *
            FROM t_user4
            WHERE c_userid = @c_userid;
        ";

        using NpgsqlCommand command =
            new NpgsqlCommand(query, _connection);

        command.Parameters.AddWithValue(
            "@c_userid",
            id
        );

        using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var user = new t_User4
            {
                c_userid =
                    Convert.ToInt32(reader["c_userid"]),

                c_name =
                    reader["c_name"].ToString()!,

                c_email =
                    reader["c_email"].ToString()!,

                c_mobile =
                    reader["c_mobile"] == DBNull.Value
                        ? ""
                        : reader["c_mobile"].ToString(),

                c_password =
                    reader["c_password"].ToString()!,

                c_gender =
                    reader["c_gender"] == DBNull.Value
                        ? ""
                        : reader["c_gender"].ToString(),

                c_hobbies =
                    reader["c_hobbies"] == DBNull.Value
                        ? ""
                        : reader["c_hobbies"].ToString(),

                c_departmentid =
                    reader["c_departmentid"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            reader["c_departmentid"]
                        ),

                c_dob = (DateOnly)(reader["c_dob"]),


                c_age =
                    reader["c_age"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            reader["c_age"]
                        ),

                c_address =
                    reader["c_address"] == DBNull.Value
                        ? ""
                        : reader["c_address"].ToString(),

                c_image =
                    reader["c_image"] == DBNull.Value
                        ? ""
                        : reader["c_image"].ToString(),

                c_status =
                    reader["c_status"] == DBNull.Value
                        ? false
                        : Convert.ToBoolean(
                            reader["c_status"]
                        )
            };

            await _connection.CloseAsync();

            return user;
        }

        await _connection.CloseAsync();

        return null;
    }


    // =====================================================
    // ADD USER
    // =====================================================

    public async Task<int> Add(t_User4 user)
    {
        await _connection.OpenAsync();

        string query = @"
            INSERT INTO t_user4
            (
                c_name,
                c_email,
                c_mobile,
                c_password,
                c_gender,
                c_hobbies,
                c_departmentid,
                c_dob,
                c_age,
                c_address,
                c_image,
                c_status
            )
            VALUES
            (
                @c_name,
                @c_email,
                @c_mobile,
                @c_password,
                @c_gender,
                @c_hobbies,
                @c_departmentid,
                @c_dob,
                @c_age,
                @c_address,
                @c_image,
                @c_status
            );
        ";

        using NpgsqlCommand command =
            new NpgsqlCommand(query, _connection);

        command.Parameters.AddWithValue(
            "@c_name",
            user.c_name
        );

        command.Parameters.AddWithValue(
            "@c_email",
            user.c_email
        );

        command.Parameters.AddWithValue(
            "@c_mobile",
            user.c_mobile ?? ""
        );

        command.Parameters.AddWithValue(
            "@c_password",
            user.c_password
        );

        command.Parameters.AddWithValue(
            "@c_gender",
            user.c_gender ?? ""
        );

        command.Parameters.AddWithValue(
            "@c_hobbies",
            user.c_hobbies ?? ""
        );

        command.Parameters.AddWithValue(
            "@c_departmentid",
            user.c_departmentid
        );

        command.Parameters.AddWithValue(
            "@c_dob",
            user.c_dob
        );

        command.Parameters.AddWithValue(
            "@c_age",
            user.c_age
        );

        command.Parameters.AddWithValue(
            "@c_address",
            user.c_address ?? ""
        );

        command.Parameters.AddWithValue(
            "@c_image",
            user.c_image ?? ""
        );

        command.Parameters.AddWithValue(
            "@c_status",
            user.c_status
        );

        int result =
            await command.ExecuteNonQueryAsync();

        await _connection.CloseAsync();

        return result;
    }


    // =====================================================
    // UPDATE USER
    // =====================================================

    public async Task<int> Update(t_User4 user)
    {
        await _connection.OpenAsync();

        string query = @"
            UPDATE t_user4
            SET
                c_name = @c_name,
                c_email = @c_email,
                c_mobile = @c_mobile,
                c_password = @c_password,
                c_gender = @c_gender,
                c_hobbies = @c_hobbies,
                c_departmentid = @c_departmentid,
                c_dob = @c_dob,
                c_age = @c_age,
                c_address = @c_address,
                c_image = @c_image,
                c_status = @c_status

            WHERE c_userid = @c_userid;
        ";

        using NpgsqlCommand command =
            new NpgsqlCommand(query, _connection);

        command.Parameters.AddWithValue(
            "@c_userid",
            user.c_userid
        );

        command.Parameters.AddWithValue(
            "@c_name",
            user.c_name
        );

        command.Parameters.AddWithValue(
            "@c_email",
            user.c_email
        );

        command.Parameters.AddWithValue(
            "@c_mobile",
            user.c_mobile ?? ""
        );

        command.Parameters.AddWithValue(
            "@c_password",
            user.c_password
        );

        command.Parameters.AddWithValue(
            "@c_gender",
            user.c_gender ?? ""
        );

        command.Parameters.AddWithValue(
            "@c_hobbies",
            user.c_hobbies ?? ""
        );

        command.Parameters.AddWithValue(
            "@c_departmentid",
            user.c_departmentid
        );

        command.Parameters.AddWithValue(
            "@c_dob",
            user.c_dob
        );

        command.Parameters.AddWithValue(
            "@c_age",
            user.c_age
        );

        command.Parameters.AddWithValue(
            "@c_address",
            user.c_address ?? ""
        );

        command.Parameters.AddWithValue(
            "@c_image",
            user.c_image ?? ""
        );

        command.Parameters.AddWithValue(
            "@c_status",
            user.c_status
        );

        int result =
            await command.ExecuteNonQueryAsync();

        await _connection.CloseAsync();

        return result;
    }


    // =====================================================
    // DELETE USER
    // =====================================================

    public async Task<int> Delete(int id)
    {
        await _connection.OpenAsync();

        string query = @"
            DELETE FROM t_user4
            WHERE c_userid = @c_userid;
        ";

        using NpgsqlCommand command =
            new NpgsqlCommand(query, _connection);

        command.Parameters.AddWithValue(
            "@c_userid",
            id
        );

        int result =
            await command.ExecuteNonQueryAsync();

        await _connection.CloseAsync();

        return result;
    }

    public async Task<t_User4?> Login(string email, string password)
    {
        await _connection.OpenAsync();

        string query = @"
        SELECT *
        FROM t_user4
        WHERE c_email = @c_email
        AND c_password = @c_password;
    ";

        using NpgsqlCommand command =
            new NpgsqlCommand(query, _connection);

        command.Parameters.AddWithValue(
            "@c_email",
            email
        );

        command.Parameters.AddWithValue(
            "@c_password",
            password
        );

        using NpgsqlDataReader reader =
            await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var user = new t_User4
            {
                c_userid =
                    Convert.ToInt32(reader["c_userid"]),

                c_name =
                    reader["c_name"].ToString()!,

                c_email =
                    reader["c_email"].ToString()!,

                c_password =
                    reader["c_password"].ToString()!,

                c_status =
                    reader["c_status"] == DBNull.Value
                        ? false
                        : Convert.ToBoolean(reader["c_status"])
            };

            await _connection.CloseAsync();

            return user;
        }

        await _connection.CloseAsync();

        return null;
    }
}