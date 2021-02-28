using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTSample.Models
{
    public partial class County
    {
        public County()
        {
            Foundation = new HashSet<Foundation>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string CountyName { get; set; }
        [Column("ProvinceID")]
        public int ProvinceId { get; set; }

        [ForeignKey(nameof(ProvinceId))]
        [InverseProperty("County")]
        public virtual Province Province { get; set; }
        [InverseProperty("County")]
        public virtual ICollection<Foundation> Foundation { get; set; }
    }
}
