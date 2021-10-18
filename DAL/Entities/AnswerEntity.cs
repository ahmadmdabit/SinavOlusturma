using DAL.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Answers")]
    public class AnswerEntity : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Answer { get; set; }
        public bool? Correctness { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public long QuestionId { get; set; }
        public virtual QuestionEntity Question { get; set; }
    }
}