using API.Helpers;
using BLL.Businesses;
using BLL.Businesses.Base;
using Common.Helpers;
using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddTransient<IDbConnection>(o => new SqliteConnection(Configuration["AppSettings:SqlConnectionString"]));

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                    });
            });
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = Configuration["Jwt:Issuer"],
                    //ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AppSettings:Secret"]))
                };
            });

            #endregion Swagger

            services.AddCors();

            services.AddControllers();

            DI(services);

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));

            //app.UseAuthorization();
            //app.UseAuthentication();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void DI(IServiceCollection services)
        {
            services.AddScoped<IRepository<UserEntity>, UserRepository>();
            services.AddScoped<IBusiness<UserEntity>, UserBusiness>();

            services.AddScoped<IRepository<ExamEntity>, ExamRepository>();
            services.AddScoped<IBusiness<ExamEntity>, ExamBusiness>();

            services.AddScoped<IRepository<QuestionEntity>, QuestionRepository>();
            services.AddScoped<IBusiness<QuestionEntity>, QuestionBusiness>();

            services.AddScoped<IRepository<AnswerEntity>, AnswerRepository>();
            services.AddScoped<IBusiness<AnswerEntity>, AnswerBusiness>();

            services.AddScoped<IRepository<UserAttendedExamEntity>, UserAttendedExamRepository>();
            services.AddScoped<IBusiness<UserAttendedExamEntity>, UserAttendedExamBusiness>();

            services.AddScoped<IRepository<SessionEntity>, SessionRepository>();
            services.AddScoped<IBusiness<SessionEntity>, SessionBusiness>();
        }
    }
}
