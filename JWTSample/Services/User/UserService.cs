using JWTSample.Helpers;
using JWTSample.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using JWTSample.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using JWTSample.ViewModels;
using JWTSample.ContosoModels;
using JWTSample.Auth;
using VakifIlan;
using System.Configuration;
using JWTSample.AuxClass;
using Microsoft.AspNetCore.Http;
using JobServiceWcf;
//using Users = JWTSample.ContosoModels.Users;

namespace JWTSample.Services.User
{
    //DI için oluşturduğumuz arayüzü implemente ediyoruz
    public class UserService<T> : IUserService<T> where T : class
    {
        private readonly AppSettings _appSettings;
        private readonly JwtTestDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // private readonly CONTOSOContext _dbContext;
        public UserService(IOptions<AppSettings> appSettings, JwtTestDBContext dbContext, IHttpContextAccessor accessor)
        //   public UserService(IOptions<AppSettings> appSettings, CONTOSOContext dbContext)
        {
            _dbContext = dbContext;
            _appSettings = appSettings.Value;
            _httpContextAccessor = accessor;
        }

        //Ekstra bir DTO veya model oluşturmamak için şimdilik değerlerimi geriye tuple olarak dönüyorum.
        // public (string username, string token)? Authenticate(string username, string password)
        #region orginal methods

        public TokenUser Authenticate(string username, string password)
        {
            //Kullanıcının gerçekten olup olmadığı kontrol ediyorum yoksa direk null dönüyorum.
            var dbUser = _dbContext.Users.SingleOrDefault(x => x.UserName == username && x.Password == password);

            if (dbUser == null)
                return null;

            Auth.User user = new Auth.User() { Email = dbUser.UserName, Name = dbUser.Name, Password = dbUser.Password, Surname = dbUser.Surname };

            MyTokenHandler tokenHandler = new MyTokenHandler(_appSettings);
            Token token = tokenHandler.CreateAccessToken();

            //_appSettings.

            //Refresh token Users tablosuna işleniyor.
            dbUser.RefreshToken = token.RefreshToken;
            dbUser.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenEndDate = dbUser.RefreshTokenEndDate;

            _dbContext.SaveChanges();

            return new TokenUser() { Token = token, User = user };

            #region original
            /*   
             *   İlk Versiyon(REfresh TOKEN olmayan)

            // Token oluşturmak için önce JwtSecurityTokenHandler sınıfından instance alıyorum.
            var _tokenHandler = new JwtSecurityTokenHandler();
            //İmza için gerekli gizli anahtarımı alıyorum.
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Özel olarak şu Claimler olsun dersek buraya ekleyebiliriz.
                Subject = new ClaimsIdentity(new[]
                {
                    //İstersek string bir property istersek ClaimsTypes sınıfının sabitlerinden çağırabiliriz.
                    new Claim("userId", user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.Email),
                    new Claim("Name",user.Name),
                    new Claim("Surname",user.Surname)
                }),
                //Tokenın hangi tarihe kadar geçerli olacağını ayarlıyoruz.
                Expires = DateTime.UtcNow.AddMinutes(15),
                //Son olarak imza için gerekli algoritma ve gizli anahtar bilgisini belirliyoruz.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //Token oluşturuyoruz.
            var _token = _tokenHandler.CreateToken(tokenDescriptor);
            //Oluşturduğumuz tokenı string olarak bir değişkene atıyoruz.
            string generatedToken = _tokenHandler.WriteToken(_token);

           // Sonuçlarımızı tuple olarak dönüyoruz.
              return (user.Email, generatedToken);
            // return ("", "");

            */
            #endregion
        }

        public TokenUser RefreshTokenLogin(string refreshToken)
        {
            if (refreshToken != null)
            {
                Models.Users dbUser = _dbContext.Users.FirstOrDefault(x => x.RefreshToken == refreshToken);
                if (dbUser != null && dbUser?.RefreshTokenEndDate > DateTime.Now)
                {
                    MyTokenHandler tokenHandler = new MyTokenHandler(_appSettings);
                    Token token = tokenHandler.CreateAccessToken();

                    dbUser.RefreshToken = token.RefreshToken;
                    dbUser.RefreshTokenEndDate = token.Expiration.AddMinutes(5);
                    _dbContext.SaveChanges();

                    Auth.User user = new Auth.User() { Email = dbUser.UserName, Name = dbUser.Name, Password = dbUser.Password, Surname = dbUser.Surname };

                    return new TokenUser() { Token = token, User = user };
                }
            }
            return null;
        }

        #endregion

        public ServiceResult<TokenUser> IlanAuthenticate(string username, string password)
        {

            try
            {
                VakifDb db = new VakifDb(_appSettings.ConnStr);

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return new ServiceResult<TokenUser>() { isSuccessfull = true, ResultCode = 1, ResultData = null, ResultExplanation = "Kullanıcı Adı ve Şifre Boş Olamaz!" };
                }

                var sonuc = db.GetKullanici(username, password);
                if (sonuc.BasariliMi)//yalnızca kullanıcı bulurusa true
                {

                    Auth.User user = new Auth.User()
                    {
                        PersonelID = int.Parse(sonuc.Veri.Rows[0]["ID"].ToString()),
                        TCKimlikNo = sonuc.Veri.Rows[0]["TCKimlikNo"].ToString(),
                        Name = sonuc.Veri.Rows[0]["Adi"].ToString(),
                        Surname = sonuc.Veri.Rows[0]["Soyadi"].ToString(),
                        DogumTarihi = (DateTime)(sonuc.Veri.Rows[0]["DogumTarihi"]),
                        DogumYeri = sonuc.Veri.Rows[0]["DogumYeri"].ToString(),
                        MedeniDurumu = sonuc.Veri.Rows[0]["MedeniHali"].ToString(),
                        Cinsiyet = sonuc.Veri.Rows[0]["Cinsiyeti"].ToString(),
                        Email = sonuc.Veri.Rows[0]["EPosta"].ToString(),
                    };

                    MyTokenHandler tokenHandler = new MyTokenHandler(_appSettings);
                    Token token = tokenHandler.CreateAccessToken();

                    db.SetRefreshToken(token, sonuc.Veri.Rows[0]["ID"].ToString());

                    return new ServiceResult<TokenUser>() { isSuccessfull = true, ResultCode = 0, ResultData = new TokenUser() { Token = token, User = user }, ResultExplanation = "Giriş Başarılı!" };

                }
                else
                {
                    return new ServiceResult<TokenUser>() { isSuccessfull = true, ResultCode = 1, ResultData = null, ResultExplanation = sonuc.Mesaj };
                }
            }
            catch (Exception exc) {
                return new ServiceResult<TokenUser>() { isSuccessfull = false, ResultCode = 2, ResultData = null, ResultExplanation = exc.Message };
            }
        }

        private string getIP()
        {
            var result = string.Empty;

            //first try to get IP address from the forwarded header
            if (_httpContextAccessor.HttpContext.Request.Headers != null)
            {
                //the X-Forwarded-For (XFF) HTTP header field is a de facto standard for identifying the originating IP address of a client
                //connecting to a web server through an HTTP proxy or load balancer

                var forwardedHeader = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
                if (!String.IsNullOrEmpty(forwardedHeader))
                    result = forwardedHeader.FirstOrDefault();
            }

            //if this header not exists try get connection remote IP address
            if (string.IsNullOrEmpty(result) && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                result = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            return result;
        }

        public ServiceResult<TokenUser> IlanRefreshTokenLogin(string refreshToken)
        {
            try
            {
                if (refreshToken != null)
                {
                    VakifDb db = new VakifDb(_appSettings.ConnStr);

                    var sonuc = db.CheckRefreshToken(refreshToken);

                    if (sonuc.Veri.Rows.Count > 0 && (DateTime)(sonuc.Veri.Rows[0]["RefreshTokenEndDate"]) > DateTime.Now)
                    {

                        MyTokenHandler tokenHandler = new MyTokenHandler(_appSettings);
                        Token token = tokenHandler.CreateAccessToken();

                        string applicantId = sonuc.Veri.Rows[0]["ID"].ToString();

                        db.SetRefreshToken(token , applicantId);

                        Auth.User user = new Auth.User()
                        {
                            PersonelID = int.Parse(sonuc.Veri.Rows[0]["ID"].ToString()),
                            TCKimlikNo = sonuc.Veri.Rows[0]["TCKimlikNo"].ToString(),
                            Name = sonuc.Veri.Rows[0]["Adi"].ToString(),
                            Surname = sonuc.Veri.Rows[0]["Soyadi"].ToString(),
                            DogumTarihi= (DateTime)(sonuc.Veri.Rows[0]["DogumTarihi"]),
                            DogumYeri = sonuc.Veri.Rows[0]["DogumYeri"].ToString(),
                            MedeniDurumu = sonuc.Veri.Rows[0]["MedeniHali"].ToString(),
                            Cinsiyet = sonuc.Veri.Rows[0]["Cinsiyeti"].ToString(),
                            Email = sonuc.Veri.Rows[0]["EPosta"].ToString(),
                        };

                        return new ServiceResult<TokenUser>() { isSuccessfull = true, ResultCode = 0, ResultData = new TokenUser() { Token = token, User = user }, ResultExplanation = "Giriş Başarılı!" };
                    }
                    else
                    {
                        return new ServiceResult<TokenUser>() { isSuccessfull = true, ResultCode = 1, ResultData = null, ResultExplanation = sonuc.Mesaj };
                    }
                }
                else
                {
                    return new ServiceResult<TokenUser>() { isSuccessfull = true, ResultCode = 1, ResultData = null, ResultExplanation = "RefreshToken boş olamaz!" }; //throw new Exception();
                }
            }
            catch (Exception exc)
            {
                return new ServiceResult<TokenUser>() { isSuccessfull = false, ResultCode = 2, ResultData = null, ResultExplanation = exc.Message };
            }
        }

        public ServiceResult<object> Signup(SignupModel model)
        {

            try
            {

                VakifDb db = new VakifDb(_appSettings.ConnStr);
                WSHelper wsHelper = new WSHelper(db, _appSettings);
                System.Globalization.CultureInfo clsCulture = new System.Globalization.CultureInfo("tr-TR");

                string resultInfo = "";

                if (model.email != model.emailrepeat)
                {
                    resultInfo = "Girilen E-Posta adresleri aynı değil.<br/>Lütfen E-Posta adreslerini tekrar giriniz.";                   
                    return new ServiceResult<object>() { isSuccessfull = true, ResultCode = 1, ResultData = null, ResultExplanation = resultInfo };
                }

                int iTcStatus = 0;
                string sResult = "";

                model.password = Guid.NewGuid().ToString().Substring(0, 8); ;
                model.IP = getIP();

                string seriNo = model.cuzdanSeri.ToUpper(clsCulture);

                if (!string.IsNullOrEmpty(model.cuzdanNo))
                {
                    seriNo = model.cuzdanSeri.ToUpper(clsCulture) + Util.CuzdanNoKontrol(model.cuzdanNo);// seriNo = model.cuzdanSeri;
                }

                //Cuzdan No Kontrol Ediliyor
                MernisSorguSonuc mernisSorguSonuc = wsHelper.CuzdanKontrol(model, model.tckn, seriNo, string.IsNullOrEmpty(model.cuzdanNo));

                // Tc ve kimlik bilgileri doğru ise
                if (mernisSorguSonuc.SonucKodu == 1)
                {
                   // MernisSorguSonuc mernisSorguSonuc = wsHelper.MernistenSorgula(model.tckn);

                    var sonuc = db.AddPersonel(
                       model.tckn,// HttpUtility.HtmlEncode(txtTcKimlikNo.Text),
                        model.name.ToUpper(clsCulture),
                       model.surname.ToUpper(clsCulture),
                        Convert.ToDateTime(mernisSorguSonuc.MernisDogumTarihi).ToString("dd/MM/yyyy"),
                        mernisSorguSonuc.MernisDogumYeri.ToUpper(clsCulture),
                        mernisSorguSonuc.MernisCinsiyet,
                        mernisSorguSonuc.MernisMedeniHal,
                        model.email.Replace("'", " "),
                        model.password,
                        model.IP);

                    if (sonuc.BasariliMi)
                    {
                        //Mail Gönderiliyor
                        sResult = wsHelper.SendMail(model);

                        //Mail gönderilebiliyorsa
                        if (sResult == "OK")
                        {
                            resultInfo = "Kayıt yapıldı.Şifreniz mail adresinize gönderildi." +
                                "Giriş sayfasından şifrenizi kullanarak giriş yapabilirsiniz.";
                        }
                        //Mail gönderilemezse
                        else
                        {
                            resultInfo = "Kayıt yapıldı ancak e-posta adresinize şifreniz gönderilemedi. " +
                                "Lütfen mail adresinizi kontrol edip şifreniz gelmedi ise Şifremi Unuttum menüsünden"+
                                "şifrenizin yeniden gönderilmesini deneyiniz!";
                          
                            db.AddPersonelSqlLog("-1", model.tckn + " __ " + sResult, model.IP);
                        }

                        return new ServiceResult<object>() { isSuccessfull = true, ResultCode = 0, ResultData = null, ResultExplanation = resultInfo };

                    }
                    else
                    {
                        if (sonuc.HataBilgi != null)
                        {
                            db.AddPersonelSqlLog("-1", model.tckn + " __ " + sResult, model.IP);

                        }
                        resultInfo = "Kayıt yapılamadı. Hata oluştu.";
                       
                        return new ServiceResult<object>() { isSuccessfull = true, ResultCode = 1, ResultData = null, ResultExplanation = resultInfo };

                    }
                }
                else if (iTcStatus == 2)
                {
                    resultInfo = @"Girilen kimlik bilgileri ile TC Kimlik numarası uyumlu değil.
                                      Bilgilerinizi iyice kontrol ettikten sonra hala aynı hatayı alıyorsanız.
                                      Nüfus bilgileri güncelliğini yitirmiş olabileceğinden Nüfus Müdürlüğünden doğru 
                                      bilgilerinizi gösteren Nüfus Cüzdanı Sureti alıp güncel bilgilerle başvurmanız gerekmektedir!";

                  
                    return new ServiceResult<object>() { isSuccessfull = true, ResultCode = 1, ResultData = null, ResultExplanation = resultInfo };
                }
                else // (iTcStatus == 0) defalut 0 dönüyor
                {
                    resultInfo = @"Girilen TC Kimlik Kartı/Belgesi Bilgileri Hatalıdır!
                              Yeni Kimlik Kartınız var ise {Yeni Kimlik Sahibiyim} seçilerek Yeni Kimlik Kartı Bilgileri ile başvuruda bulunulmalıdır.
                              Yeni Kimlik Kartına Başvurmuş iseniz ve kimlik kartınızı teslim almadıysanız Geçici Kimlik Belgesi üzerinde yer alan bilgiler ile başvuruda bulunmanız gerekmektedir.
                              Eski Kimlik Kartınız mevcut ve geçerli ise girdiğiniz bilgileri dikkatlice kontrol ediniz                            
                              Belirtilen Hususlara Dikkat ederek Lütfen Tekrar Deneyiniz.";

                    return new ServiceResult<object>() { isSuccessfull = true, ResultCode = 1, ResultData = null, ResultExplanation = resultInfo };
                }

            }
            catch (Exception exc)
            {
                return new ServiceResult<object>() { isSuccessfull = false, ResultCode = 2, ResultData = null, ResultExplanation = exc.Message };
            }
        }

        public ServiceResult<IseAlimTalebi[]> IlanListesi() {

            try
            {
                VakifDb db = new VakifDb(_appSettings.ConnStr);
                WSHelper wsHelper = new WSHelper(db, _appSettings);
                IseAlimTalebi[] liste = wsHelper.IlanListesi();
                return new ServiceResult<IseAlimTalebi[]>() { isSuccessfull = true, ResultCode = 1, ResultData = liste, ResultExplanation = "Liste Başarılı" };
            }
            catch (Exception exc) {
                return new ServiceResult<IseAlimTalebi[]>() { isSuccessfull = false, ResultCode = 2, ResultData = null, ResultExplanation = exc.Message };
            }
        }

        public ServiceResult<string> SaveApplication(IsBasvurusuBilgileri basvuru) {

            System.Globalization.CultureInfo clsCulture = new System.Globalization.CultureInfo("tr-TR");
            VakifDb db = new VakifDb(_appSettings.ConnStr);
            WSHelper wsHelper = new WSHelper(db, _appSettings);
            var sonuc = db.GetKullanici(basvuru.PersonelID);
            basvuru.TcKimlikNo = sonuc.Veri.Rows[0]["TCKimlikNo"].ToString();
            basvuru.EPosta = sonuc.Veri.Rows[0]["EPosta"].ToString().Replace("'", " ");
            BusinessLayer bHelper = new BusinessLayer(db, _appSettings, basvuru);            
            
            //ServiceResult<string> osymResult = bHelper.checkOsymPuan();
            //if (osymResult.ResultCode == 1)
            //    return osymResult;           
            
            try
            {                
              
                string[] sAddress = new string[2];

                IsBasvurusuKisiselBilgiler clsKisiselBilgiler = new IsBasvurusuKisiselBilgiler();
                IsBasvurusuDigerBilgiler clsDigerBilgiler = new IsBasvurusuDigerBilgiler();               
                IsBasvurusu clsIsBasvuru = new IsBasvurusu();

                islemSonucu clsIslemSonucu = new islemSonucu();
                mesaj clsMesaj = new mesaj();

                clsKisiselBilgiler.tcKimlikNoField = sonuc.Veri.Rows[0]["TCKimlikNo"].ToString();
                clsKisiselBilgiler.adField = sonuc.Veri.Rows[0]["Adi"].ToString();
                clsKisiselBilgiler.soyadField = sonuc.Veri.Rows[0]["Soyadi"].ToString();
                clsKisiselBilgiler.dogumYeriField = sonuc.Veri.Rows[0]["DogumYeri"].ToString();
                clsKisiselBilgiler.dogumTarihiField = (DateTime)(sonuc.Veri.Rows[0]["DogumTarihi"]);
                clsKisiselBilgiler.medeniDurumuField = Util.getMedeniHalEnumFromStr(sonuc.Veri.Rows[0]["MedeniHali"].ToString());
                clsKisiselBilgiler.cinsiyetField = Util.getCinsiyetEnumFromStr(sonuc.Veri.Rows[0]["Cinsiyeti"].ToString());

                ServiceResult<string[]> adresResult = bHelper.checkAdresCode();
                if (adresResult.ResultCode == 1)
                    return  new ServiceResult<string>()
                    {
                        isSuccessfull = false,
                        ResultCode = 1,
                        ResultData = null,
                        ResultExplanation = "Sistemden adres bilginize ulaşılamamıştır. İkametgah şartı isteyen ilanlara başvurabilmek için nüfus müdürlüğüne gidip adres kaydınızı yaptırmanız gerekmektedir."
                    };

                ilceBilgisi clsIlceBilgisi = new ilceBilgisi();
                clsIlceBilgisi.mernisIlceKoduFieldSpecified = true;               
                clsIlceBilgisi.ilceAdiField = adresResult.ResultData[0].ToString();
                clsIlceBilgisi.mernisIlceKoduField = int.Parse(adresResult.ResultData[1].ToString());
                clsKisiselBilgiler.ilceField = clsIlceBilgisi;
               
               
                clsDigerBilgiler.ePostaField = sonuc.Veri.Rows[0]["EPosta"].ToString().Replace("'", " ");

                if (!string.IsNullOrEmpty(basvuru.EvTelNumarasi))
                {
                    clsDigerBilgiler.evTelNumarasiField = basvuru.EvTelNumarasi.Replace("'", " ");
                }
                if (!string.IsNullOrEmpty(basvuru.IsTelNumarasi))
                {
                    clsDigerBilgiler.isTelNumarasiField = basvuru.IsTelNumarasi.Replace("'", " ");
                }
                if (!string.IsNullOrEmpty(basvuru.CepTelNumarasi))
                {
                    clsDigerBilgiler.cepTelNumarasiField = basvuru.CepTelNumarasi.Replace("'", " ");
                }

                clsDigerBilgiler.kpssGirisYiliField = Convert.ToInt32(basvuru.KpssGirisYili);
                clsDigerBilgiler.kpssPuaniField = Convert.ToDouble(basvuru.KpssPuani); //76,44835
                clsDigerBilgiler.universiteBolumuField = basvuru.UniversiteBolumu.Replace("'", " ").ToUpper(clsCulture);               

                clsDigerBilgiler.egitimDurumuField = Util.getEgitimDurumEnumFromStr(basvuru.EgitimDurumu);
                clsDigerBilgiler.egitimDurumuFieldSpecified = true;

                clsDigerBilgiler.mezuniyetTarihiField = Convert.ToDateTime(basvuru.MezuniyetTarihi);
                clsDigerBilgiler.mezuniyetTarihiFieldSpecified = true;


                if (!string.IsNullOrEmpty(basvuru.TecilTarihi))
                {
                    clsDigerBilgiler.tecilTarihiField = Convert.ToDateTime(basvuru.TecilTarihi);
                    clsDigerBilgiler.tecilTarihiFieldSpecified = true;
                }
                
                clsDigerBilgiler.askerlikDurumuField =  Util.getAskerlikEnumFromStr(basvuru.AskerlikDurumu);

                clsIsBasvuru.digerBilgilerField = clsDigerBilgiler;
                clsIsBasvuru.kisiselBilgilerField = clsKisiselBilgiler;

                ServiceResult<string> kaydetResult = bHelper.basvuruKaydet(clsIsBasvuru, basvuru.IlanID);

                bHelper.UpdatePersonData(basvuru, adresResult.ResultData);

                return kaydetResult; // new ServiceResult<string>() { isSuccessfull = true, ResultCode = 0, ResultData = null, ResultExplanation = "Başvuru Kaydedildi!" };

            }
            catch (Exception exc)
            {
                return new ServiceResult<string>() { isSuccessfull = false, ResultCode = 2, ResultData = null, ResultExplanation = exc.Message };
            }

        }

        public ServiceResult<Application[]> BasvuruListesi(string personelID) {


            try
            {
                VakifDb db = new VakifDb(_appSettings.ConnStr);
                WSHelper wsHelper = new WSHelper(db, _appSettings);
               
                 var sonuc = db.GetKullanici(personelID);
                string TCNo =  sonuc.Veri.Rows[0]["TCKimlikNo"].ToString();
                Application[] liste = wsHelper.BasvuruListesi(TCNo);              

                return new ServiceResult<Application[]>() { isSuccessfull = true, ResultCode = 1, ResultData = liste, ResultExplanation = "Liste Başarılı" };
            }
            catch (Exception exc)
            {
                return new ServiceResult<Application[]>() { isSuccessfull = false, ResultCode = 2, ResultData = null, ResultExplanation = exc.Message };
            }
        }


        public Ingredients GetIngredients()
        {
            return new Ingredients() { Bacon = 0, Meat = 0, Cheese = 0, Salad = 0 };
        }

        public List<Models.Users> getUserList()
        {
            List<Models.Users> items = _dbContext.Users.ToList();// AçıkKapıSosyalHizmetBaş1.Where(item => item.Id == id.ToString()).FirstOrDefault();
            return items;

        }

        public List<Order> GetOrders()
        {

            List<Order> orders = new List<Order>()
            {
                new Order()
                {
                    Price = 11,
                    OrderData = new OrderData()
                    {
                        Street = "bultr",
                        DeliveryMethod = "fastest",
                        Email = "a@a.com",
                        Zipcode = "123456",
                        Name = "ali",
                        Country = "TR"
                    },
                    Ingredients = new Ingredients()
                    {
                        Bacon = 1,
                        Meat = 2,
                        Salad = 1,
                        Cheese = 1
                    }

                }

            };

            return orders;
        }

    }
}

