using DAL.Entities;
using DAL.Repositories.Base;
using System.Data;

namespace DAL.Repositories
{
    public class ExamRepository : BaseRepository<ExamEntity>
    {
        public ExamRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}