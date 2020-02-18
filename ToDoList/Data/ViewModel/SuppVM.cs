using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class SuppVM
    {
        public IEnumerable<SuppVM> data { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int length { get; set; }
        public int filterlength { get; set; }
    }
}
