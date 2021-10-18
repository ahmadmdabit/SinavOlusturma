using DAL.Entities;
using DAL.Repositories.Base;
using System.Data;

namespace DAL.Repositories
{
    public class QuestionRepository : BaseRepository<QuestionEntity>
    {
        public QuestionRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}