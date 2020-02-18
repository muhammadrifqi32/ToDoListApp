using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Base
{
    public class BaseModel2
    {
        public bool isDelete { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }
    }
}
