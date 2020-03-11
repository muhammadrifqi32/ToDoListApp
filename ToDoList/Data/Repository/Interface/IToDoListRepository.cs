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
        Task<IEnumerable<ToDoListVM>> Get();
        Task<IEnumerable<ToDoListVM>> Get(string Id, int status);
        IEnumerable<ToDoListVM> GetStatus(string UserId);
        int Create(ToDoListVM toDoListVM);
        int Update(int Id, ToDoListVM toDoListVM);
        int Delete(int Id);
        int Checkedlist(int Id, ToDoList toDoList);
        int Uncheckedlist(int Id, ToDoList toDoList);
        //Task<IEnumerable<ToDoListVM>> Search(int Id, int status, string keyword);
        //Task<IEnumerable<ToDoListVM>> Paging(int Id, int status, string keyword, int pageSize, int pageNumber);
        Task<ToDoListVM> PageSearch(string Id, int status, string keyword, int pageSize, int pageNumber);
    }
}
