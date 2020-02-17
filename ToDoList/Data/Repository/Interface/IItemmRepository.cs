using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace Data.Repository.Interface
{
    public interface IItemmRepository
    {
        Task<IEnumerable<ItemmVM>> Get();
        Task<IEnumerable<ItemmVM>> Get(int Id);
        int Create(ItemmVM itemmVM);
        int Update(int Id, ItemmVM itemmVM);
        int Delete(int Id);
    }
}
