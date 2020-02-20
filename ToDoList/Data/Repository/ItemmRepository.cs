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
    public class ItemmRepository : IItemmRepository
    {

        public IConfiguration _configuration;
        public readonly ConnectionStrings _connectionStrings;

        public ItemmRepository(ConnectionStrings connectionStrings, IConfiguration configuration)
        {
            _configuration = configuration;
            this._connectionStrings = connectionStrings; //samewith dependency injection?
        }

        DynamicParameters parameters = new DynamicParameters();

        public int Create(ItemmVM itemmVM)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_InsertItemm"; //callsp
                parameters.Add("@Name", itemmVM.name); 
                parameters.Add("@Stock", itemmVM.stock); 
                parameters.Add("@Price", itemmVM.price); 
                parameters.Add("@SuppID", itemmVM.SuppId); 

                var users = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure); //toiinputdatausingdapper
                return users;
            }
        }

        public int Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_DeleteItemm";
                parameters.Add("@ID", Id);

                var users = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return users;
            }
        }

        public async Task<IEnumerable<ItemmVM>> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetAllItemm"; //

                var itemm = await connection.QueryAsync<ItemmVM>(procName, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
                return itemm;
            }
        }

        public async Task<IEnumerable<ItemmVM>> Get(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetItemmById"; //
                parameters.Add("@ID", Id);

                var itemm = await connection.QueryAsync<ItemmVM>(procName, parameters, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
                return itemm;
            }
        }

        public int Update(int Id, ItemmVM itemmVM)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_UpdateItemm";
                parameters.Add("@ID", Id);
                parameters.Add("@Name", itemmVM.name);
                parameters.Add("@Stock", itemmVM.stock);
                parameters.Add("@Price", itemmVM.price);
                parameters.Add("@SuppID", itemmVM.SuppId);

                var itemm = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure); //toiinputdatausingdapper
                return itemm;
            }
        }

        public async Task<ItemmVM> PageSearch(string keyword, int pageSize, int pageNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_ItemmPageSearch";
                parameters.Add("@SearchKey", keyword);
                parameters.Add("@pageSize", pageSize);
                parameters.Add("@pageNumber", pageNumber);
                parameters.Add("@length", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@filterlength", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var result = new ItemmVM();

                result.data = await connection.QueryAsync<ItemmVM>(procName, parameters, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
                int filterlength = parameters.Get<int>("@filterlength");
                result.filterlength = filterlength;
                int length = parameters.Get<int>("@length");
                result.length = length;
                return result;
            }
        }
    }
}
