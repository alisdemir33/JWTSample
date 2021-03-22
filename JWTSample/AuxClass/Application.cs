using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.AuxClass
{
    public class Application
    {
        public long iseBasvurusuNo
        {
            get; set;
        }

        public string unvan { get; set; }


        public string ilAdi { get; set; }


        public string ilceAdi { get; set; }


        public string baslik { get; set; }


        /// <remarks/>
        public string belgeTeslimTarihi 
        {
            get; set;
        }      

        /// <remarks/>
        public System.DateTime basvuruTarihi
        {
            get ;   set ;
        }

        public string basvuruDurumu
        {
            get ;  set ;
        }


        /// <remarks/>
        public string tcKimlikNo
        {
            get; set;
        }
    }
}
