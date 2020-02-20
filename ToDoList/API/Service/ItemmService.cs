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
    public class ItemmService : IItemmService
    {
        private IItemmRepository _itemmRepository;

        public ItemmService() { }

        public ItemmService(IItemmRepository itemmRepository)
        {
            _itemmRepository = itemmRepository;
        }

        public int Create(ItemmVM itemmVM)
        {
            return _itemmRepository.Create(itemmVM);
        }

        public int Delete(int Id)
        {
            return _itemmRepository.Delete(Id);
        }

        public Task<IEnumerable<ItemmVM>> Get()
        {
            return _itemmRepository.Get();
        }

        public Task<IEnumerable<ItemmVM>> Get(int Id)
        {
            return _itemmRepository.Get(Id);
        }

        public int Update(int Id, ItemmVM itemmVM)
        {
            return _itemmRepository.Update(Id,itemmVM);
        }
        public Task<ItemmVM> PageSearch(string keyword, int pageSize, int pageNumber)
        {
            return _itemmRepository.PageSearch(keyword, pageSize, pageNumber);
        }
    }
}
