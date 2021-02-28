using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTSample.ContosoModels
{
    public partial class Enrollment
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Column("CourseID")]
        public Guid CourseId { get; set; }
        [Column("StudentID")]
        public Guid StudentId { get; set; }
        public int? Grade { get; set; }

        [ForeignKey(nameof(CourseId))]
        [InverseProperty("Enrollment")]
        public virtual Course Course { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty(nameof(Instructor.Enrollment))]
        public virtual Instructor Student { get; set; }
        [ForeignKey(nameof(StudentId))]
        [InverseProperty("Enrollment")]
        public virtual Student StudentNavigation { get; set; }
    }
}
