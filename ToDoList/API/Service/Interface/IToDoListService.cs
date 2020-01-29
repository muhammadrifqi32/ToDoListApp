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
        Task<IEnumerable<ToDoListVM>> Get();
        Task<IEnumerable<ToDoListVM>> Get(int Id, int status);
        int Create(ToDoListVM toDoListVM);
        int Update(int Id, ToDoListVM toDoListVM);
        int Delete(int Id);
    }
}
