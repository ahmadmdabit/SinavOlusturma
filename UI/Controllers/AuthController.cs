using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DAL.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Common.Helpers;
using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace UI.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;
        protected readonly IActionContextAccessor _accessor;

        public AuthController(ILogger<AuthController> logger, IHttpClientFactory clientFactory, IActionContextAccessor accessor)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient();
            _accessor = accessor;
        }

        [UI.Helpers.AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var role = Encoding.UTF8.GetString(StringToByteArray(_accessor.ActionContext.HttpContext.Request.Cookies["Role"]));
            if (!string.IsNullOrWhiteSpace(role))
            {
                return Redirect(role == "Admin" ? "/Home" : "/Student/Exams");
            }

            await _accessor.ActionContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [UI.Helpers.AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewData["Errors"] = "Invalid login attempt...";
                return View();
            }

            var user = await Auth(username, password);

            if (user == null)
            {
                ViewData["Errors"] = "Invalid login attempt...";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim("username", username),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await _accessor.ActionContext.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            CookieOptions option = new CookieOptions { Expires = DateTime.Now.AddDays(7) };
            Response.Cookies.Append("Token", user.Token, option);
            Response.Cookies.Append("UserId", user.Id.ToString(), option);
            //Response.Cookies.Append("Role", Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Role)), option);
            Response.Cookies.Append("Role", ByteArrayToString(Encoding.UTF8.GetBytes(user.Role)), option);
            
            _logger.LogInformation("User {Id} logged in at {Time}.", user.Id, DateTime.UtcNow);

            return Redirect(user.Role == "Admin" ? "/Home" : "/Student/Exams");
        }

        async Task<UserEntity> Auth(string username, string password)
        {
            UserEntity user = null;
            var baseUrl = "https://localhost:44393/api/User/Authenticate";
            var request = CreateGetRequest(baseUrl, HttpMethod.Post);
            request.Content = new StringContent(JsonConvert.SerializeObject(new { Username = username, Password = password }), Encoding.UTF8, "application/json");
            var response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<ApiResult<UserEntity>>(responseString).Data;
                _accessor.ActionContext.HttpContext.Items["User"] = user;
                _accessor.ActionContext.HttpContext.Items["Token"] = user?.Token;
                //_accessor.ActionContext.HttpContext.Request.Headers["Authorization"] = $"Bearer { user?.Token}";
            }
            return user;
        }

        HttpRequestMessage CreateGetRequest(string url, HttpMethod method)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Add("Accept", "application/json");
            return request;
        }

        [UI.Helpers.Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User {Name} logged out at {Time}.",
                User.Identity.Name, DateTime.UtcNow);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _accessor.ActionContext.HttpContext.Response.Cookies.Delete("Token");
            _accessor.ActionContext.HttpContext.Response.Cookies.Delete("UserId");
            _accessor.ActionContext.HttpContext.Response.Cookies.Delete("Role");

            return Redirect("/Auth/Login");
        }

        static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        static byte[] StringToByteArray(String hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                return new byte[] { };
            }
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
