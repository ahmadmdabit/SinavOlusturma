using DAL.Entities;
using DAL.Repositories.Base;
using System.Data;

namespace DAL.Repositories
{
    public class AnswerRepository : BaseRepository<AnswerEntity>
    {
        public AnswerRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}