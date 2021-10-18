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
    public class AnswerController : BaseApiController<AnswerEntity>
    {
        public AnswerController(IBusiness<AnswerEntity> business, ILogger<BaseApiController<AnswerEntity>> logger, IActionContextAccessor accessor) : base(business, logger, accessor)
        {
        }

        [API.Helpers.Authorize(Roles = "Admin, User")]
        // GET: api/[controller]/ByQuestion/5
        [HttpGet("ByQuestion/{questionId}")]
        public async Task<ActionResult<ApiResult<IEnumerable<AnswerEntity>>>> GetByQuestion(long questionId)
        {
            this._logger.LogInformation($"[GetByQuestion:{questionId}] [{this._ip}]");
            var entities = await this._business.GetAsync("QuestionId", questionId).ConfigureAwait(false);
            if (entities != null)
            {
                return Ok(new ApiResult<IEnumerable<AnswerEntity>>(true, entities));
            }
            return Ok(new ApiResult<IEnumerable<AnswerEntity>>(false, null, new NotFoundResult().StatusCode, "NotFound"));
        }
    }
}