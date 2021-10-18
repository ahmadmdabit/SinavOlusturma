using BLL.Models;
using Common.Helpers;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;
        protected readonly IActionContextAccessor _accessor;
        private UserEntity _user;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, IActionContextAccessor accessor)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient();
            _accessor = accessor;

            Auth().Wait();
        }

        [UI.Helpers.Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            ViewData["Token"] = _accessor.ActionContext.HttpContext.Request.Cookies["Token"];
            return View(await GetExams());
        }

        [UI.Helpers.Authorize(Roles = "Admin, User")]
        [HttpGet("Student/Exams")]
        public async Task<IActionResult> StudentExams()
        {
            return View("StudentExams", await GetStudentExams());
        }

        [UI.Helpers.Authorize(Roles = "Admin, User")]
        [HttpGet("Student/Exam/{id}")]
        public async Task<IActionResult> StudentExam(long id)
        {
            ViewData["Token"] = _accessor.ActionContext.HttpContext.Request.Cookies["Token"];
            ViewData["UserId"] = _accessor.ActionContext.HttpContext.Request.Cookies["UserId"];
            ViewData["ExamId"] = id;
            return View("StudentExam", await GetExam(id));
        }

        [UI.Helpers.Authorize(Roles = "Admin")]
        [HttpGet("Show/{id}")]
        public async Task<IActionResult> Show(long id)
        {
            ViewData["Token"] = _accessor.ActionContext.HttpContext.Request.Cookies["Token"];
            return View("Show", await GetExam(id));
        }

        [UI.Helpers.Authorize(Roles = "Admin")]
        [HttpGet("New")]
        public async Task<IActionResult> New()
        {
            ViewData["Token"] = _accessor.ActionContext.HttpContext.Request.Cookies["Token"];
            ViewData["RandomDataList"] = await GetRandomData();
            var model = new ExamEntity();
            model.Questions.Add(new QuestionEntity());
            model.Questions.Add(new QuestionEntity());
            model.Questions.Add(new QuestionEntity());
            model.Questions.Add(new QuestionEntity());
            model.Questions.ForEach(x =>
            {
                x.Answers.Add(new AnswerEntity());
                x.Answers.Add(new AnswerEntity());
                x.Answers.Add(new AnswerEntity());
                x.Answers.Add(new AnswerEntity());
            });
            return View("New", model);
        }

        [UI.Helpers.Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(long id)
        {
            ViewData["Token"] = _accessor.ActionContext.HttpContext.Request.Cookies["Token"];
            return View("Edit", await GetExam(id));
        }

        [UI.Helpers.Authorize(Roles = "Admin")]
        [HttpPost("Save")]
        public async Task<ApiResult<ExamEntity>> Save([FromBody] dynamic examEntity)
        {
            return await SaveExam(examEntity);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        async Task Auth()
        {
            var baseUrl = "https://localhost:44393/api/User/Authenticate";
            var request = CreateGetRequest(baseUrl, HttpMethod.Post);
            request.Content = new StringContent(JsonConvert.SerializeObject(new { Username = "admin", Password = "321" }), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                _user = JsonConvert.DeserializeObject<ApiResult<UserEntity>>(responseString).Data;
                _accessor.ActionContext.HttpContext.Items["User"] = _user;
                _accessor.ActionContext.HttpContext.Items["Token"] = _user?.Token;
                _accessor.ActionContext.HttpContext.Request.Headers["Authorization"] = $"Bearer { _user?.Token}";
            }
        }

        async Task<ApiResult<ExamEntity>> SaveExam(ExamEntity examEntity)
        {
            ApiResult<ExamEntity> result = null;
            var baseUrl = "https://localhost:44393/api/Exam";
            var request = CreateGetRequest(baseUrl, HttpMethod.Post);
            request.Content = new StringContent(JsonConvert.SerializeObject(examEntity), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<ApiResult<ExamEntity>>(responseString);
            }
            return result;
        }

        async Task<List<ExamEntity>> GetStudentExams()
        {
            List<ExamEntity> exams = null;
            var baseUrl = "https://localhost:44393/api/UserExams";
            var request = CreateGetRequest(baseUrl, HttpMethod.Get);
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                exams = JsonConvert.DeserializeObject<ApiResult<List<ExamEntity>>>(responseString).Data;
                exams.ForEach(x =>
                {
                    x.Content = null;
                });
            }
            return exams;
        }

        async Task<List<ExamEntity>> GetExams()
        {
            List<ExamEntity> exams = null;
            var baseUrl = "https://localhost:44393/api/Exam";
            var request = CreateGetRequest(baseUrl, HttpMethod.Get);
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                exams = JsonConvert.DeserializeObject<ApiResult<List<ExamEntity>>>(responseString).Data;
                exams.ForEach(x =>
                {
                    x.Content = null;
                });
            }
            return exams;
        }

        async Task<ExamEntity> GetExam(long id)
        {
            ExamEntity exam = null;
            var baseUrl = "https://localhost:44393/api/Exam/"+id;
            var request = CreateGetRequest(baseUrl, HttpMethod.Get);
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                exam = JsonConvert.DeserializeObject<ApiResult<ExamEntity>>(responseString).Data;
            }
            return exam;
        }

        async Task<List<RandomData>> GetRandomData()
        {
            List<RandomData> randomData = null;
            var baseUrl = "https://localhost:44393/api/Exam/RandomData/5";
            var request = CreateGetRequest(baseUrl, HttpMethod.Get);
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                randomData = JsonConvert.DeserializeObject<ApiResult<List<RandomData>>>(responseString).Data;
            }
            return randomData;
        }

        HttpRequestMessage CreateGetRequest(string url, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {_user?.Token}");
            return request;
        }
    }
}
