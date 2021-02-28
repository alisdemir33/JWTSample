using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTSample.Models
{
    public partial class Users
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Surname { get; set; }
        [Column("TCKN")]
        [StringLength(11)]
        public string Tckn { get; set; }
        [StringLength(500)]
        public string RefreshToken { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RefreshTokenEndDate { get; set; }
    }
}
