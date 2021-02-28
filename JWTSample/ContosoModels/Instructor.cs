using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTSample.ContosoModels
{
    public partial class Instructor
    {
        public Instructor()
        {
            CourseInstructor = new HashSet<CourseInstructor>();
            Department = new HashSet<Department>();
            Enrollment = new HashSet<Enrollment>();
        }

        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? HireDate { get; set; }
        [Required]
        [StringLength(128)]
        public string Discriminator { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(OfficeAssignment.Instructor))]
        public virtual OfficeAssignment IdNavigation { get; set; }
        [InverseProperty("Instructor")]
        public virtual ICollection<CourseInstructor> CourseInstructor { get; set; }
        [InverseProperty("Instructor")]
        public virtual ICollection<Department> Department { get; set; }
        [InverseProperty("Student")]
        public virtual ICollection<Enrollment> Enrollment { get; set; }
    }
}
