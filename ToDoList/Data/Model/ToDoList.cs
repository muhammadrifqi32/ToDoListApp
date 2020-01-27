using System;
using System.Collections.Generic;
using System.Text;
using Data.Base;
using Data.ViewModel;

namespace Data.Model
{
    public class ToDoList : BaseModel
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public User User { get; set; }

        public ToDoList()
        {

        }

        public ToDoList(ToDoListVM toDoListVM)
        {
            this.Id = toDoListVM.Id;
            this.Name = toDoListVM.Name;
            this.Status = toDoListVM.Status;
            this.CreateDate = DateTimeOffset.Now;
            this.isDelete = false;
        }
        public void Update(ToDoListVM toDoListVM)
        {
            this.Id = toDoListVM.Id;
            this.Name = toDoListVM.Name;
            this.Status = toDoListVM.Status;
            this.UpdateDate = DateTimeOffset.Now;
        }
        public void Delete(ToDoListVM toDoListVM)
        {
            this.isDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }
    }
}
