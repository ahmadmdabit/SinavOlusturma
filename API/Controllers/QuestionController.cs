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
    public class QuestionController : BaseApiController<QuestionEntity>
    {
        public QuestionController(IBusiness<QuestionEntity> business, ILogger<BaseApiController<QuestionEntity>> logger, IActionContextAccessor accessor) : base(business, logger, accessor)
        {
        }
    }
}