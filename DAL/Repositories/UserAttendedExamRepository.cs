using DAL.Entities;
using DAL.Repositories.Base;
using System.Data;

namespace DAL.Repositories
{
    public class UserAttendedExamRepository : BaseRepository<UserAttendedExamEntity>
    {
        public UserAttendedExamRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }
    }
}