using API.Controllers.Base;
using BLL.Businesses;
using BLL.Businesses.Base;
using Common.Helpers;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UserAttendedExamController : BaseApiController<UserAttendedExamEntity>
    {
        public UserAttendedExamController(IBusiness<UserAttendedExamEntity> business, ILogger<BaseApiController<UserAttendedExamEntity>> logger, IActionContextAccessor accessor) : base(business, logger, accessor)
        {
        }

        [API.Helpers.Authorize(Roles = "Admin, User")]
        // GET: api/[controller]/UserAttendedExamAnswers/5
        [HttpGet("/UserAttendedExamAnswers/{examId}")]
        public async Task<ActionResult<ApiResult<IEnumerable<UserAttendedExamEntity>>>> GetUserAttendedExamAnswers(long examId)
        {
            this._logger.LogInformation($"[GetUserAttendedExamAnswers:{examId}] [{this._ip}]");
            if (this._user is UserEntity user && user != null)
            {
                return Ok(new ApiResult<IEnumerable<UserAttendedExamEntity>>(true, await (this._business as UserAttendedExamBusiness).GetUserAttendedExamAnswersAsync(user.Id, examId).ConfigureAwait(false)));
            }
            return Ok(new ApiResult<IEnumerable<UserAttendedExamEntity>>(false, null, new BadRequestResult().StatusCode, "BadRequest"));
        }
    }
}