using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTSample.ContosoModels
{
    public partial class CourseInstructor
    {
        [Key]
        [Column("CourseID")]
        public Guid CourseId { get; set; }
        [Key]
        [Column("InstructorID")]
        public Guid InstructorId { get; set; }

        [ForeignKey(nameof(CourseId))]
        [InverseProperty("CourseInstructor")]
        public virtual Course Course { get; set; }
        [ForeignKey(nameof(InstructorId))]
        [InverseProperty("CourseInstructor")]
        public virtual Instructor Instructor { get; set; }
    }
}
