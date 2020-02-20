using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class ItemmVM
    {
        public IEnumerable<ItemmVM> data { get; set; }
        public int Id { get; set; }
        public string name { get; set; }
        public int stock { get; set; }
        public int price { get; set; }
        public int SuppId { get; set; }
        public string suppname { get; set; }
        public int length { get; set; }
        public int filterlength { get; set; }
    }
}
