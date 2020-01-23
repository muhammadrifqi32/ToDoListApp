using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace Data.Repository.Interface
{
    public interface IToDoListRepository
    {
        Task<IEnumerable<Model.ToDoList>> Get();
        Task<IEnumerable<Model.ToDoList>> Get(int Id);
        int Create(ToDoListVM toDoListVM);
        int Update(int Id, ToDoListVM toDoListVM);
        int Delete(int Id);
    }
}
