using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTSample.ContosoModels
{
    public partial class OfficeAssignment
    {
        [Key]
        [Column("InstructorID")]
        public Guid InstructorId { get; set; }
        [StringLength(50)]
        public string Location { get; set; }

        [InverseProperty("IdNavigation")]
        public virtual Instructor Instructor { get; set; }
    }
}
