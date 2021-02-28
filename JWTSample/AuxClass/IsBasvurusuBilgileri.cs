using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.AuxClass
{
   
    public  class IsBasvurusuBilgileri 
    {
        public string PersonelID { get; set; }
        public long IlanID { get; set; }
        public string BasvuruNo { get; set; }
        public string IlanIlceID { get; set; }
        public string IlanIlID { get; set; }
        public string UnvanID { get; set; }

        public string Ip { get; set; }
       // public string IlceID{ get; set; }
        public DateTime DogumTarihi { get; set; }
        public string DogumYeri { get; set; }
        public string TcKimlikNo { get; set; }
        public string MedeniDurumu { get; set; }
        public string Ad { get; set; }
        public string Cinsiyet { get; set; }
        public string Soyad { get; set; }
        public string EgitimDurumu { get; set; }
        public string EPosta { get; set; }
        public string EvTelNumarasi { get; set; }
        public string KpssGirisYili { get; set; }
        public string KpssPuani { get; set; }
        public string AskerlikDurumu { get; set; }
        public string CepTelNumarasi { get; set; }
        public string TecilTarihi { get; set; }
        public string IsTelNumarasi { get; set; }
        public string UniversiteBolumu { get; set; }
        public string MezuniyetTarihi { get; set; }

    }
}
