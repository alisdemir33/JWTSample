using System.ComponentModel.DataAnnotations;

namespace JWTSample
{
    //Authenticate işleminde neler geleceğini ve validasyonları için oluşturdum.
    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class SignupModel {

        [Required]
        public string tckn { get; set; }

        [Required]
        public string name { get; set; }
        [Required]
        public string surname { get; set; }

        [Required]
        public string email { get; set; }
        [Required]
        public string emailrepeat { get; set; }
       
        public string cuzdanSeri { get; set; }
       
        public string cuzdanNo { get; set; }  
        
        public string IP { get; set; }

        public string password { get; set; }


    }


    public class RefreshTokenEntity {
        private string refreshToken;

        public string RefreshToken { get => refreshToken; set => refreshToken = value; }
    }
}
