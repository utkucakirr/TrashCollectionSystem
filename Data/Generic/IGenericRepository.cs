using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Generic
{
    public interface IGenericRepository<T> where T:class
    {
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(long id);
        Task<T> GetById(long id);
        Task<IEnumerable<T>> GetAll();
    }
}
