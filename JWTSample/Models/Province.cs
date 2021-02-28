using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTSample.Models
{
    public partial class Province
    {
        public Province()
        {
            County = new HashSet<County>();
            Foundation = new HashSet<Foundation>();
        }

        [Key]
        [Column("ProvinceID")]
        public int ProvinceId { get; set; }
        [StringLength(50)]
        public string ProvinceName { get; set; }

        [InverseProperty("Province")]
        public virtual ICollection<County> County { get; set; }
        [InverseProperty("Province")]
        public virtual ICollection<Foundation> Foundation { get; set; }
    }
}
