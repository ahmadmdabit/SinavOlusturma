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
    public class QuestionBusiness : BaseBusiness<QuestionEntity>
    {
        public QuestionBusiness(IRepository<QuestionEntity> repository) : base(repository)
        {
        }
    }
}