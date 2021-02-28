using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.AuxClass
{
    public class PersonelInfo
    {
        public long TCKimlikNo { get; set; }
        public DateTime LogTime { get; set; }
        public string MernisAd { get; set; }
        public string MernisSoyad { get; set; }
        public string MernisSeriNo { get; set; }
        public string LogType { get; set; }
        public string BasvuranAd { get; set; }
        public string BasvuranSoyad { get; set; }
        public string BasvuranSeriNo { get; set; }
    }
}
