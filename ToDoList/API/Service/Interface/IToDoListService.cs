using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace API.Service.Interface
{
    public interface IToDoListService
    {
        Task<IEnumerable<Data.Model.ToDoList>> Get();
        Task<IEnumerable<Data.Model.ToDoList>> Get(int Id);
        int Create(ToDoListVM toDoListVM);
        int Update(int Id, ToDoListVM toDoListVM);
        int Delete(int Id);
    }
}
