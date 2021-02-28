using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTSample.ContosoModels
{
    public partial class Foundation
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Required]
        [Column("FName")]
        [StringLength(50)]
        public string Fname { get; set; }
        [Column("CountyID")]
        public int CountyId { get; set; }
        [Column("ProvinceID")]
        public int ProvinceId { get; set; }
        [StringLength(50)]
        public string Address { get; set; }
        [StringLength(15)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Fax { get; set; }
        [Column("EMail")]
        [StringLength(50)]
        public string Email { get; set; }

        [ForeignKey(nameof(CountyId))]
        [InverseProperty("Foundation")]
        public virtual County County { get; set; }
        [ForeignKey(nameof(ProvinceId))]
        [InverseProperty("Foundation")]
        public virtual Province Province { get; set; }
    }
}
