using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;

namespace Data.Repository.Interface
{
    public interface ISuppRepository
    {
        Task<IEnumerable<Supp>> Get();
        Task<IEnumerable<Supp>> Get(int Id);
        int Create(SuppVM suppVM);
        int Update(int Id, SuppVM suppVM);
        int Delete(int Id);
        Task<SuppVM> PageSearch(string keyword, int pageSize, int pageNumber);
    }
}
