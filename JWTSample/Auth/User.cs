using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.Auth
{
    public class User
    {      
        public int PersonelID{ get; set; }
        public string TCKimlikNo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Cinsiyet { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string DogumYeri { get; set; }
        public string MedeniDurumu { get; set; }

        public string Email { get; set; }
        public string Password { get; set; } 

        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
    }
}
