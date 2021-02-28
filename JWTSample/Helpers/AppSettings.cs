namespace JWTSample.Helpers
{
    public class AppSettings
    {
        public string SecretKey { get; set; }
        public string ConnStr { get; set; }

        public string Mail_Gonderen { get; set; }
        public string Mail_User_Name { get; set; }
        public string Mail_User_Password { get; set; }
        public string Mail_Host { get; set; }
        public int TokenExpirationTimeInMinutes { get; set; }
        public int RefreshTokenExpirationTimeInMinutes {get;set; }
        public string Nvi_User_Name { get; set; }
        public string Nvi_Password { get; set; }
        public string btnUName { get; set; }
        public string btnPwd { get; set; }

    }
}
