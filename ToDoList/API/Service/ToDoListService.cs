using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Service.Interface;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;

namespace API.Service
{
    public class ToDoListService : IToDoListService
    {
        private IToDoListRepository _todolistRepository;

        public ToDoListService() { }

        public ToDoListService(IToDoListRepository todolistRepository)
        {
            _todolistRepository = todolistRepository;
        }

        public int Create(ToDoListVM toDoListVM)
        {
            return _todolistRepository.Create(toDoListVM);
            //throw new NotImplementedException();
        }

        public int Delete(int Id)
        {
            return _todolistRepository.Delete(Id);
            //throw new NotImplementedException();
        }

        public Task<IEnumerable<ToDoListVM>> Get()
        {
            return _todolistRepository.Get();
            //throw new NotImplementedException();
        }

        public Task<IEnumerable<ToDoListVM>> Get(int Id, int status)
        {
            return _todolistRepository.Get(Id, status);
            //throw new NotImplementedException();
        }

        public int Update(int Id, ToDoListVM toDoListVM)
        {
            return _todolistRepository.Update(Id, toDoListVM);
            //throw new NotImplementedException();
        }

        public int Checkedlist(int Id, ToDoList toDoList)
        {
            return _todolistRepository.Checkedlist(Id, toDoList);
        }

        public int Uncheckedlist(int Id, ToDoList toDoList)
        {
            return _todolistRepository.Uncheckedlist(Id, toDoList);
        }

        public Task<IEnumerable<ToDoListVM>> Search(int Id, int status, string keyword)
        {
            return _todolistRepository.Search(Id, status, keyword);
        }

        public Task<IEnumerable<ToDoListVM>> Paging(int Id, int status, string keyword, int pageSize, int pageNumber)
        {
            return _todolistRepository.Paging(Id, status, keyword, pageSize, pageNumber);
        }
    }
}
