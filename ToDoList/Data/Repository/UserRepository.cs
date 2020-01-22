using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.ConnectionString;
using Data.Context;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;
using Microsoft.Data.SqlClient;

namespace Data.Repository
{
    public class UserRepository : IUserRepository
    {
        public readonly ConnectionStrings _connectionStrings;

        public UserRepository(ConnectionStrings connectionStrings)
        {
            this._connectionStrings = connectionStrings; //samewith dependency injection?
        }

        DynamicParameters parameters = new DynamicParameters();

        public int Create(UserVM userVM)
        {
            using(SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_InsertUser"; //callsp
                parameters.Add("@Username", userVM.Username); //retrieve username
                parameters.Add("@Password", userVM.Username); //retrieve password

                var users = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure); //toiinputdatausingdapper
                return users;
            }
            //throw new NotImplementedException();
        }

        public int Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_DeleteUser";
                parameters.Add("@ID", Id);

                var users = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return users;
            }
            //throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetAllUser"; //

                var users = await connection.QueryAsync<User>(procName, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
                return users;
            }
            //throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> Get(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetUserById"; //
                parameters.Add("@ID", Id);

                var users = await connection.QueryAsync<User>(procName, parameters, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
                return users;
            }
            //throw new NotImplementedException();
        }

        public int Update(int Id, UserVM userVM)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_UpdateUser";
                parameters.Add("@ID", Id);
                parameters.Add("@Username", userVM.Username);
                parameters.Add("@Password", userVM.Username);

                var users = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure); //toiinputdatausingdapper
                return users;
            }
            //throw new NotImplementedException();
        }
    }
}
