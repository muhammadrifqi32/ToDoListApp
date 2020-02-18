using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Base;

namespace Data.Repository.Interface
{
    public interface IRepository<T> where T : class, IEntity //every need ID will access IEntity
    {
        //T to call model dynamically

        Task<IEnumerable<T>> Get();
        Task<T> Get(int id);
        Task<T> Post(T entity);
        Task<T> Put(T entity);
        Task<T> Delete(int id);
    }
}
