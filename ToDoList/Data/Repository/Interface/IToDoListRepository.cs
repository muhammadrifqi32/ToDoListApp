﻿using System;
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
        Task<IEnumerable<ToDoListVM>> Get(int Id, int status);
        int Create(ToDoListVM toDoListVM);
        int Update(int Id, ToDoListVM toDoListVM);
        int Delete(int Id);
        int Checkedlist(int Id, ToDoList toDoList);
        int Uncheckedlist(int Id, ToDoList toDoList);
    }
}
