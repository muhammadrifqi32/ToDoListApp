using System;
using System.Collections.Generic;
using System.Text;
using Data.Base;
using Data.ViewModel;

namespace Data.Model
{
    public class Supp : BaseModel
    {
        public string Name { get; set; }

        public Supp() { }

        public Supp(SuppVM suppVM)
        {
            this.Name = suppVM.Name;
            this.CreateDate = DateTimeOffset.Now;
            this.isDelete = false;
        }

        public void Update(SuppVM suppVM)
        {
            this.Name = suppVM.Name;
            this.UpdateDate = DateTimeOffset.Now;
        }

        public void Delete(SuppVM suppVM)
        {
            this.DeleteDate = DateTimeOffset.Now;
            this.isDelete = true;
        }
    }
}
