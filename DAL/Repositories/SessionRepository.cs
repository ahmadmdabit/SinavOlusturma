using DAL.Entities;
using DAL.Repositories.Base;
using System.Data;

namespace DAL.Repositories
{
    public class SessionRepository : BaseRepository<SessionEntity>
    {
        public SessionRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}