using System;
using System.Collections.Generic;

namespace DAL.Entities.Base
{
    public interface IEntity
    {
        object GetKeyValue();
        string GetTableName();
    }
}