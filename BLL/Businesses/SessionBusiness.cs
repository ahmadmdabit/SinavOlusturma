using BLL.Businesses.Base;
using BLL.Models;
using Common.Extensions;
using Common.Helpers;
using DAL.Entities;
using DAL.Repositories.Base;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Businesses
{
    public class SessionBusiness : BaseBusiness<SessionEntity>
    {
        public SessionBusiness(IRepository<SessionEntity> repository) : base(repository)
        {
        }
    }
}