using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace API.Service.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<User>> Get();
        Task<IEnumerable<User>> Get(int Id);
        int Create(UserVM userVM);
        int Update(int Id, UserVM userVM);
        int Delete(int Id);
        User Get(UserVM userVM);
    }
}
