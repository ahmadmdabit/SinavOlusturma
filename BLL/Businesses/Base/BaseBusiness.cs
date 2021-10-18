using DAL.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Businesses.Base
{
    public abstract class BaseBusiness<T> : IBusiness<T> where T : class
    {
        protected readonly IRepository<T> _repository;

        protected BaseBusiness(IRepository<T> repository)
        {
            this._repository = repository;
        }

        public virtual async Task<IEnumerable<T>> GetAsync()
        {
            return await this._repository.GetAsync().ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> GetAsync(string prop, object val)
        {
            return await this._repository.GetAsync(prop, val).ConfigureAwait(false);
        }

        public virtual async Task<T> GetAsync(long id)
        {
            return await this._repository.GetAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<bool> DeleteAsync(long id)
        {
            return await this._repository.DeleteAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<int> AddAsync(IEnumerable<T> entities)
        {
            return await this._repository.InsertAsync(entities).ConfigureAwait(false);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            return await this._repository.InsertAsync(entity).ConfigureAwait(false);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            return await this._repository.UpdateAsync(entity).ConfigureAwait(false);
        }
    }
}