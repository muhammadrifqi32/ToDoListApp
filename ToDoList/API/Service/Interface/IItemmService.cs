using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace API.Service.Interface
{
    public interface IItemmService
    {
        Task<IEnumerable<ItemmVM>> Get();
        Task<IEnumerable<ItemmVM>> Get(int Id);
        int Create(ItemmVM itemmVM);
        int Update(int Id, ItemmVM itemmVM);
        int Delete(int Id);
        Task<ItemmVM> PageSearch(string keyword, int pageSize, int pageNumber);
    }
}
