using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Npgsql;
using Test.Models;

namespace Test.Repository.Services
{
    public class precticeServices
    {
        private readonly NpgsqlConnection _connection;

        public precticeServices(NpgsqlConnection connection)
        {
            _connection = connection;
        }


        public async Task<List<User?>> GetAll()
        {
            var users = new List<User>();

            try
            {
                await _connection.OpenAsync();
                var command = new NpgsqlCommand("SELECT * FROM t_userapi", _connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("c_userid")),
                        Name = reader.GetString(reader.GetOrdinal("c_username")),
                        Email = reader.GetString(reader.GetOrdinal("c_email")),
                        Password = reader.GetString(reader.GetOrdinal("c_password"))

                    });
                }
                return users;
            }
            catch (Exception e)
            {
                return users;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<User> GetUser(int id)
        {
            try
            {
                await _connection.OpenAsync();
                using var cmd = new NpgsqlCommand(@"SELECT * FROM t_userapi where c_userid = @id", _connection);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new User
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("c_userid")),
                        Name = reader.GetString(reader.GetOrdinal("c_username")),
                        Email = reader.GetString(reader.GetOrdinal("c_email")),
                        Password = reader.GetString(reader.GetOrdinal("c_password"))
                    };
                }
                return new User();
            }
            catch (Exception e)
            {
                return new User();

            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task<bool> AddUser(User data)
        {

            try
            {
                //check email
                await _connection.OpenAsync();
                var query = @"INSERT INTO t_userapi (c_email,c_password, c_username) 
                            VALUES (@email, @password, @name)";
                await using var cmd = new NpgsqlCommand(query, _connection);
                cmd.Parameters.AddWithValue("@name", data.Name);
                cmd.Parameters.AddWithValue("@email", data.Email);
                cmd.Parameters.AddWithValue("@password", data.Password);

                int reader = await cmd.ExecuteNonQueryAsync();
                return reader > 0;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }

        }
        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                await _connection.OpenAsync();

                using var cmd1 = new NpgsqlCommand(@"select * from t_userapi  WHERE c_userid = @id", _connection);
                cmd1.Parameters.AddWithValue("@id", user.Id);
                using var reader = await cmd1.ExecuteReaderAsync();
                if (!await reader.ReadAsync()) return false;

                using var cmd = new NpgsqlCommand(@"UPDATE t_userapi SET c_username = @name, c_email = @email, c_password = @password WHERE c_userid = @id", _connection);
                cmd.Parameters.AddWithValue("@id", user.Id);
                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
        public async Task DeteleUser(int id)
        {
            try
            {
                await _connection.OpenAsync();
                using var cmd = new NpgsqlCommand(@"DELETE FROM t_userapi WHERE c_userid = @id", _connection);
                await cmd.ExecuteNonQueryAsync();
                return;
            }
            catch (Exception e)
            {
                return;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> Save(User user)
        {
            if (user.Id == 0)
            {
                return await AddUser(user);
            }

            return await UpdateUser(user);
        }

    }
}