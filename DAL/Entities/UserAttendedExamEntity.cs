using DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("UserAttendedExams")]
    public class UserAttendedExamEntity : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ExamId { get; set; }
        public long QuestionId { get; set; }
        public long AnswerId { get; set; }
        public bool Correctness { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}