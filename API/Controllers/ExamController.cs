using API.Controllers.Base;
using BLL.Businesses;
using BLL.Businesses.Base;
using BLL.Models;
using Common.Helpers;
using DAL.Entities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace API.Controllers
{
    public class ExamController : BaseApiController<ExamEntity>
    {
        private readonly IHttpClientFactory _clientFactory;
        public ExamController(IBusiness<ExamEntity> business, ILogger<BaseApiController<ExamEntity>> logger, IActionContextAccessor accessor, IHttpClientFactory clientFactory) : base(business, logger, accessor)
        {
            _clientFactory = clientFactory;
        }

        [API.Helpers.Authorize(Roles = "Admin, User")]
        // GET: api/[controller]/UserExams
        [HttpGet("UserExams")]
        public async Task<ActionResult<ApiResult<IEnumerable<ExamEntity>>>> GetUserAttendedExams()
        {
            this._logger.LogInformation($"[GetUserAttendedExams] [{this._ip}]");
            if (this._user is UserEntity user && user != null)
            {
                return Ok(new ApiResult<IEnumerable<ExamEntity>>(true, await (this._business as ExamBusiness).GetUserAttendedExamsAsync(user.Id).ConfigureAwait(false)));
            }
            return Ok(new ApiResult<IEnumerable<ExamEntity>> (false, null, new BadRequestResult().StatusCode, "BadRequest"));
        }

        [API.Helpers.Authorize(Roles = "Admin, User")]
        // GET: api/[controller]/UserExams/5
        [HttpGet("UserExams/{examId}")]
        public async Task<ActionResult<ApiResult<ExamEntity>>> GetUserExam(long examId)
        {
            this._logger.LogInformation($"[GetUserExam:{examId}] [{this._ip}]");
            var entity = await (this._business as ExamBusiness).GetUserCleanExamAsync(examId).ConfigureAwait(false);
            if (entity != null)
            {
                return Ok(new ApiResult<ExamEntity>(true, entity));
            }
            return this.NotFoundApi();
        }

        [API.Helpers.Authorize(Roles = "Admin, User")]
        // GET: api/[controller]/RandomData/5
        [HttpGet("RandomData/{count}")]
        public async Task<ActionResult<ApiResult<Object>>> GetRandomData(int count = 5)
        {
            this._logger.LogInformation($"[GetRandomData:{count}] [{this._ip}]");

            var stories = new List<RandomData>();

            var client = _clientFactory.CreateClient();

            var baseUrl = "https://www.wired.com";
            var request = CreateGetRequest(baseUrl);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var html = await response.Content.ReadAsStringAsync();
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var aNodes = doc.DocumentNode.SelectNodes("//a[starts-with(@href,'/story')]"); //"//td/input"
                foreach (var item in aNodes)
                {
                    var link = baseUrl + item.Attributes["href"].Value;
                    if (stories.Any(x => x.Link == link))
                    {
                        continue;
                    }
                    request = CreateGetRequest(link);
                    response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var html2 = await response.Content.ReadAsStringAsync();
                        var doc2 = new HtmlDocument();
                        doc2.LoadHtml(html2);
                        var titleNode = doc2.DocumentNode.SelectSingleNode("//h1[starts-with(@data-testid,'ContentHeaderHed')]");
                        var contentNodes = doc2.DocumentNode.SelectNodes("//div[starts-with(@data-journey-hook,'client-content')]");
                        var contents = string.Empty;
                        foreach (var itm in contentNodes)
                        {
                            contents += itm.InnerHtml;
                        }
                        stories.Add(new RandomData { Title = titleNode.InnerText, Content = contents, Link = link });
                    }
                    if (stories.Count == count)
                    {
                        break;
                    }
                }
                /*

                 var stories = [];
                    $('a[href^="/story"]').each(function(k, v){
                     if(-1===stories.indexOf($(v).attr('href'))) {
                        stories.push($(v).attr('href'))
                     }
                    })
                    console.log(stories)
                 */
            }

            return Ok(new ApiResult<Object>(true, stories));

            HttpRequestMessage CreateGetRequest(string url)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml");
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36");
                return request;
            }

        }
    }

    
}