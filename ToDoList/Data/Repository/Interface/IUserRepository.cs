using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace Data.Repository.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> Get();
        Task<IEnumerable<User>> Get(int Id);
        int Create(UserVM userVM);
        int Update(int Id, UserVM userVM);
        int Delete(int Id);
    }
}
