using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.AuxClass
{
    public class Application
    {
        public long iseBasvurusuNo { get; set; }
        public string unvan { get; set; }
        public string ilAdi { get; set; }
        public string ilceAdi { get; set; }
        public string baslik { get; set; }
        public string belgeTeslimTarihi { get; set; }
        public System.DateTime basvuruTarihi { get; set; }
        public string basvuruDurumu { get; set; }
        public string tcKimlikNo { get; set; }

        public string EgitimDurumu { get; set; }
        public string UniversiteBolum { get; set; }
        public string MezuniyetTarihi { get; set; }

        public string KpssGirisYili { get; set; }
        public string KpssPuani { get; set; }

        public string AskerlikDurumu { get; set; }
        public string TecilTarihi { get; set; }

        public string CepTelefonu { get; set; }
        public string EvTelNumarasi { get; set; }
        public string IsTelNumarasi { get; set; }
        public string EPosta { get; set; }

    }
}
