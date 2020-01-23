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

namespace Data.Repository
{
    public class ToDoListRepository : IToDoListRepository
    {
        public readonly ConnectionStrings _connectionStrings;

        public ToDoListRepository(ConnectionStrings connectionStrings)
        {
            this._connectionStrings = connectionStrings; //samewith dependency injection?
        }

        DynamicParameters parameters = new DynamicParameters();

        public int Create(ToDoListVM toDoListVM)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_InsertToDoList"; //callsp
                parameters.Add("@Name", toDoListVM.Name); //retrieve username
                parameters.Add("@Status", toDoListVM.Status); //retrieve password
                //parameters.Add("@UserId", toDoListVM.User); //retrieve password

                var todolist = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure); //toiinputdatausingdapper
                return todolist;
            }
            //throw new NotImplementedException();
        }

        public int Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_DeleteToDoList";
                parameters.Add("@ID", Id);

                var todolist = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return todolist;
            }
            //throw new NotImplementedException();
        }

        public async Task<IEnumerable<Model.ToDoList>> Get()
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetAllToDoList"; //

                var todolist = await connection.QueryAsync<Model.ToDoList>(procName, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
                return todolist;
            }
            //throw new NotImplementedException();
        }

        public async Task<IEnumerable<Model.ToDoList>> Get(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_GetToDoListByID"; //
                parameters.Add("@ID", Id);

                var todolist = await connection.QueryAsync<Model.ToDoList>(procName, parameters, commandType: CommandType.StoredProcedure); //await ada jeda. bermanfaat untuk banyak data
                return todolist;
            }
            //throw new NotImplementedException();
        }

        public int Update(int Id, ToDoListVM toDoListVM)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStrings.Value))
            {
                var procName = "SP_UpdateToDoList";
                parameters.Add("@ID", Id);
                parameters.Add("@Name", toDoListVM.Name);
                parameters.Add("@Status", toDoListVM.Status);
                parameters.Add("@UserId", toDoListVM.User);

                var todolist = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure); //toiinputdatausingdapper
                return todolist;
            }
            //throw new NotImplementedException();
        }
    }
}
