using System;
using System.Collections.Generic;
using System.Text;
using Data.Base;

namespace Data.Model
{
    public class Department : BaseModel2, IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
