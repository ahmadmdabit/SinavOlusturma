using DAL.Repositories.Base;
using System.Data;
using DAL.Entities;

namespace DAL.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>
    {
        public UserRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}