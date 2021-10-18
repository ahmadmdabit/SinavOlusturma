using DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Exams")]
    public class ExamEntity : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual List<UserEntity> Users { get; set; }
        public virtual List<QuestionEntity> Questions { get; set; }
        public ExamEntity()
        {
            if (Users == null)
            {
                Users = new List<UserEntity>();
            }
            if (Questions == null)
            {
                Questions = new List<QuestionEntity>();
            }
        }
    }
}