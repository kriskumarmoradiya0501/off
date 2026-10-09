using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Test.Models;

namespace Test.Repository.Services
{
    public class RegisterService
    {
        private readonly NpgsqlConnection _conn;
        public RegisterService(NpgsqlConnection conn) => _conn = conn;

        public async Task<bool> RegisterUser (User data)
        {

            try
            {
                //check email
                await _conn.OpenAsync();
                var query = @"INSERT INTO t_userapi (c_email,c_password, c_username) 
                            VALUES (@email, @password, @name)";
                await using var cmd = new NpgsqlCommand(query, _conn);
                cmd.Parameters.AddWithValue("@name", data.Name);
                cmd.Parameters.AddWithValue("@email", data.Email);
                cmd.Parameters.AddWithValue("@password", data.Password);

                int reader =  await cmd.ExecuteNonQueryAsync();
                    return reader>0;
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            finally
            {
                await _conn.CloseAsync();
            }
            
        }

        public async Task<bool> LoginUser (vm_Login data)
        {
            try{
                await _conn.OpenAsync();
            var query = "SELECT 1 FROM t_userapi WHERE c_email = @email and c_password = @password";
            await using var cmd = new NpgsqlCommand(query, _conn);
            cmd.Parameters.AddWithValue("@email", data.Email);
            cmd.Parameters.AddWithValue("@password", data.Password);
            using var reader = await cmd.ExecuteReaderAsync();

            if(reader.HasRows) return true;

            return false;
            
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            finally
            {
                await _conn.CloseAsync();
            }


        }
        public async Task<bool> LoginAdmin (vm_Login data)
        {
            try{
                await _conn.OpenAsync();
            var query = "SELECT 1 FROM t_admins WHERE c_email = @email and c_password = @password";
            await using var cmd = new NpgsqlCommand(query, _conn);
            cmd.Parameters.AddWithValue("@email", data.Email);
            cmd.Parameters.AddWithValue("@password", data.Password);
            using var reader = await cmd.ExecuteReaderAsync();

            if(reader.HasRows) return true;

            return false;
            
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            finally
            {
                await _conn.CloseAsync();
            }


        }
    }
}
