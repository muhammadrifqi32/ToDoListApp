using System;
using System.Collections.Generic;
using System.Text;
using Data.Context;
using Data.Model;

namespace Data.Repository.Data
{
    public class DepartmentRepository : GeneralRepository<Department, MyContext> //implement from general repository class (CRUD) and set the model name.
    {
        public DepartmentRepository(MyContext myContext) : base(myContext)
        {

        }

        //insert other method
    }
}
