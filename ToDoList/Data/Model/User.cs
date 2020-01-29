using System;
using System.Collections.Generic;
using System.Text;
using Data.Base;
using Data.ViewModel;

namespace Data.Model
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User() { }

        public User(UserVM userVM)
        {
            this.Email = userVM.Email;
            this.Username = userVM.Username;
            this.Password = userVM.Password;
            this.CreateDate = DateTimeOffset.Now;
            this.isDelete = false;
        }

        public void Update(UserVM userVM)
        {
            this.Email = userVM.Email;
            this.Username = userVM.Username;
            this.Password = userVM.Password;
            this.UpdateDate = DateTimeOffset.Now;
        }

        public void Delete(UserVM userVM)
        {
            this.DeleteDate = DateTimeOffset.Now;
            this.isDelete = true;
        }
    }
}
