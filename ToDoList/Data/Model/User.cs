using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Data.Base;
using Data.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Data.Model
{
    public class User : IdentityUser
    {
        public bool isDelete { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }
    }
}
