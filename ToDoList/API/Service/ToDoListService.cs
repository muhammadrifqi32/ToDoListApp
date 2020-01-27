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

        public Task<IEnumerable<ToDoListVM>> Get(int Id)
        {
            return _todolistRepository.Get(Id);
            //throw new NotImplementedException();
        }

        public int Update(int Id, ToDoListVM toDoListVM)
        {
            return _todolistRepository.Update(Id, toDoListVM);
            //throw new NotImplementedException();
        }
    }
}
