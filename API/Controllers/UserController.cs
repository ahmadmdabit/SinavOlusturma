using API.Controllers.Base;
using BLL.Businesses.Base;
using Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BLL.Businesses;
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

namespace API.Controllers
{

    public class UserController : BaseApiController<UserEntity>
    {
        private readonly AppSettings _appSettings;
        private readonly IBusiness<SessionEntity> _sessionBusiness;
        public UserController(IBusiness<UserEntity> business, ILogger<BaseApiController<UserEntity>> logger, IActionContextAccessor accessor, IOptions<AppSettings> options, IBusiness<SessionEntity> sessionBusiness) : base(business, logger, accessor)
        {
            _appSettings = options.Value;
            _sessionBusiness = sessionBusiness;
        }

        [API.Helpers.AllowAnonymous]
        // POST: api/[controller]/Register
        [HttpPost("Register")]
        public async Task<ActionResult<ApiResult<UserEntity>>> PostRegister([FromBody] RegisterModel model)
        {
            this._logger.LogInformation($"[PostRegister] [{this._ip}] {JsonConvert.SerializeObject(model)}");
            var entity = await (this._business as UserBusiness).RegisterAsync(model).ConfigureAwait(false);
            if (entity != null)
            {
                return Ok(new ApiResult<UserEntity>(true, null));
            }
            return this.BadRequestApi();
        }

        [API.Helpers.AllowAnonymous]
        // POST: api/[controller]/Authenticate
        [HttpPost("Authenticate")]
        public async Task<ActionResult<ApiResult<UserEntity>>> PostAuthenticate([FromBody] AuthenticateModel model)
        {
            this._logger.LogInformation($"[PostAuthenticate] [{this._ip}] {JsonConvert.SerializeObject(model)}");
            var entity = await (this._business as UserBusiness).AuthenticateAsync(model).ConfigureAwait(false);
            if (entity != null)
            {
                entity.PasswordHash = null;
                entity.PasswordSalt = null;
                entity.Token = TokenGenerate(entity.Id, entity.Role, out DateTime expires);
                try
                {
                    await this._sessionBusiness.AddAsync(new SessionEntity { UserId = entity.Id, Token = entity.Token, Expires = expires }).ConfigureAwait(false);
                }
                catch (Exception exc)
                {
                }
                return Ok(new ApiResult<UserEntity>(true, entity));
            }
            return this.BadRequestApi();
        }

        private string TokenGenerate(long id, string role, out DateTime expires)
        {
            expires = DateTime.UtcNow.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim("id", id.ToString()), new Claim("role", role.ToString()) }),
                       // new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, id.ToString()) }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}