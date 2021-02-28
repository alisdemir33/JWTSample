using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VakifIlan
{
    public class MernisSorguSonuc
    {
        public string TCKimlikNo { get; set; }        
        public string MernisAd { get; set; }
        public string MernisSoyad { get; set; }
        public string MernisMedeniHal { get; set; }
        public string MernisCinsiyet { get; set; }
        public string MernisDogumTarihi { get; set; }
        public string MernisDogumYeri { get; set; }
        public int SonucKodu { get; set; }

    }
}