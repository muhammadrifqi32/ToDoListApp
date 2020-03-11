using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class ToDoListVM
    {
        public IEnumerable<ToDoListVM> data { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public int length { get; set; }
        public int filterlength { get; set; }
        public int Total { get; set; }
    }
}
