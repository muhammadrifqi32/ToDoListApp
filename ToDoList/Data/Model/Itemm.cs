using System;
using System.Collections.Generic;
using System.Text;
using Data.Base;
using Data.ViewModel;

namespace Data.Model
{
    public class Itemm : BaseModel
    {
        public string name { get; set; }
        public int stock { get; set; }
        public int price { get; set; }
        public Supp Supp { get; set; }

        public Itemm() { }

        public Itemm(ItemmVM itemmVM)
        {
            this.name = itemmVM.name;
            this.stock = itemmVM.stock;
            this.price = itemmVM.price;
            this.CreateDate = DateTimeOffset.Now;
            this.isDelete = false;
        }
        public void Update (ItemmVM itemmVM)
        {
            this.name = itemmVM.name;
            this.stock = itemmVM.stock;
            this.price = itemmVM.price;
            this.UpdateDate = DateTimeOffset.Now;
        }
        public void Delete (ItemmVM itemmVM)
        {
            this.DeleteDate = DateTimeOffset.Now;
            this.isDelete = true;
        }
    }
}
