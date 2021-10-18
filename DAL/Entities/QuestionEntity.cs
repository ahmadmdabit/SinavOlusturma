using DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Questions")]
    public class QuestionEntity : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Question { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public long ExamId { get; set; }
        public virtual ExamEntity Exam { get; set; }
        public virtual List<AnswerEntity> Answers { get; set; }
        public QuestionEntity()
        {
            if (Answers == null)
            {
                Answers = new List<AnswerEntity>();
            }
        }
    }
}