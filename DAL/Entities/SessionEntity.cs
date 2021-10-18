using DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Sessions")]
    public class SessionEntity : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? CreatedAt { get; set; }

        public long UserId { get; set; }
        public virtual List<UserEntity> Users { get; set; }
        public SessionEntity()
        {
            if (Users == null)
            {
                Users = new List<UserEntity>();
            }
        }
    }
}