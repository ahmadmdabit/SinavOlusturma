using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Businesses.Base
{
    public interface IBusiness<T>
    {
        Task<IEnumerable<T>> GetAsync();

        Task<IEnumerable<T>> GetAsync(string prop, object val);

        Task<T> GetAsync(long id);

        Task<bool> DeleteAsync(long id);

        Task<int> AddAsync(IEnumerable<T> entities);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);
    }
}