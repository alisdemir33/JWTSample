using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTSample.Models
{
    [Table("'Açık Kapı Sosyal Hizmet Baş_1$'")]
    public partial class AçıkKapıSosyalHizmetBaş1
    {
        [Column("ID")]
        [StringLength(255)]
        public string Id { get; set; }
        [StringLength(255)]
        public string İl { get; set; }
        [StringLength(255)]
        public string İlçe { get; set; }
        [Column("TC Kimlik No")]
        [StringLength(255)]
        public string TcKimlikNo { get; set; }
        [Column("Cep Tel#No")]
        [StringLength(255)]
        public string CepTelNo { get; set; }
        [StringLength(255)]
        public string Adı { get; set; }
        [StringLength(255)]
        public string Soyadı { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BasvuruTarihi { get; set; }
        [StringLength(255)]
        public string BasvuruKonu { get; set; }
    }
}
