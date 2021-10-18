using DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        /// <summary>
        ///  Roles: Admin, User
        /// </summary>
        public string Role { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual List<ExamEntity> Exams { get; set; }
        public UserEntity()
        {
            if (Exams == null)
            {
                Exams = new List<ExamEntity>();
            }
        }
    }
}