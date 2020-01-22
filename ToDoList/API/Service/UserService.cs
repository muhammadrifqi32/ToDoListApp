using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Service.Interface;
using Data.Model;
using Data.Repository.Interface;
using Data.ViewModel;

namespace API.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService() { }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public int Create(UserVM userVM)
        {
            return _userRepository.Create(userVM);
            //throw new NotImplementedException();
        }

        public int Delete(int Id)
        {
            return _userRepository.Delete(Id);
            //throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> Get()
        {
            return _userRepository.Get();
            //throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> Get(int Id)
        {
            return _userRepository.Get(Id);
            //throw new NotImplementedException();
        }

        public int Update(int Id, UserVM userVM)
        {
            return _userRepository.Update(Id, userVM);
            //throw new NotImplementedException();
        }
    }
}
