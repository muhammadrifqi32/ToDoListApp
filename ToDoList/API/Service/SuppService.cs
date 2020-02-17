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
    public class SuppService : ISuppService
    {
        private ISuppRepository _suppRepository;

        public SuppService() { }

        public SuppService(ISuppRepository suppRepository)
        {
            _suppRepository = suppRepository;
        }

        public int Create(SuppVM suppVM)
        {
            return _suppRepository.Create(suppVM);
        }

        public int Delete(int Id)
        {
            return _suppRepository.Delete(Id);
        }

        public Task<IEnumerable<Supp>> Get()
        {
            return _suppRepository.Get();
        }

        public Task<IEnumerable<Supp>> Get(int Id)
        {
            return _suppRepository.Get(Id);
        }

        public int Update(int Id, SuppVM suppVM)
        {
            return _suppRepository.Update(Id, suppVM);
        }
    }
}
