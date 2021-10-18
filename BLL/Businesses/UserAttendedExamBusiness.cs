using BLL.Businesses.Base;
using Common.Extensions;
using Common.Helpers;
using DAL.Entities;
using DAL.Repositories.Base;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BLL.Businesses
{
    public class UserAttendedExamBusiness : BaseBusiness<UserAttendedExamEntity>
    {
        public UserAttendedExamBusiness(IRepository<UserAttendedExamEntity> repository) : base(repository)
        {
        }

        public async Task<IEnumerable<UserAttendedExamEntity>> GetUserAttendedExamAnswersAsync(long userId, long examId)
        {
            return await this._repository.QueryAsync($@"
                SELECT * FROM UserAttendedExams
                WHERE ExamId = {examId} AND UserId = {userId};", null).ConfigureAwait(false);
        }
    }
}