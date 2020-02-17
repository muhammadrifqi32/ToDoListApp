using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.ConnectionString;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.Repository
{
    public class SuppRepository : ISuppRepository
    {

        public IConfiguration _configuration;
        public readonly ConnectionStrings _connectionStrings;

        public SuppRepository(ConnectionStrings connectionStrings, IConfiguration configuration)
        {
            _configuration = configuration;
            this._connectionStrings = connectionStrings; //samewith dependency injection?
        }

        DynamicParameters parameters = new DynamicParameters();

        public int Create(SuppVM suppVM)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                    var procName = "SP_InsertSupp"; //callsp
                    parameters.Add("@Name", suppVM.Name); //retrieve username

                    var users = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure); //toiinputdatausingdapper
                    return users;
            }
        }

        public int Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_DeleteSupps";
                parameters.Add("@ID", Id);

                var users = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return users;
            }
        }

        public async Task<IEnumerable<Supp>> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetAllSupp"; //

                var supps = await connection.QueryAsync<Supp>(procName, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
                return supps;
            }
        }

        public async Task<IEnumerable<Supp>> Get(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetSuppsById"; //
                parameters.Add("@ID", Id);

                var supps = await connection.QueryAsync<Supp>(procName, parameters, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
                return supps;
            }
        }

        public int Update(int Id, SuppVM suppVM)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_UpdateSupps";
                parameters.Add("@ID", Id);
                parameters.Add("@Name", suppVM.Name);

                var supps = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure); //toiinputdatausingdapper
                return supps;
            }
        }
    }
}
