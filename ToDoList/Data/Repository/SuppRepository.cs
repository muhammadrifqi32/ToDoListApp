﻿using System;
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

        public IEnumerable<Supp> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetAllSupp"; //

                var supps = connection.Query<Supp>(procName, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
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

        public async Task<SuppVM> PageSearch(string keyword, int pageSize, int pageNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_SuppPageSearch";
                parameters.Add("@SearchKey", keyword);
                parameters.Add("@pageSize", pageSize);
                parameters.Add("@pageNumber", pageNumber);
                parameters.Add("@length", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@filterlength", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var result = new SuppVM();

                result.data = await connection.QueryAsync<SuppVM>(procName, parameters, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
                int filterlength = parameters.Get<int>("@filterlength");
                result.filterlength = filterlength;
                int length = parameters.Get<int>("@length");
                result.length = length;
                return result;
            }
        }
    }
}
