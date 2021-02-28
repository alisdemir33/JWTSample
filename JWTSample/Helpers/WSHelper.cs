//using ButunlesikService;
using JWTSample.AuxClass;
using MernisTc;
//using ServiceReference1;
using JobServiceWcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Threading.Tasks;
using VakifIlan;
using System.Data;

namespace JWTSample.Helpers
{
    public class WSHelper
    {
        VakifDb db;

        AppSettings _appSettings;
        Mail clsMail = new Mail();

        public WSHelper(VakifDb dbParam, AppSettings appSettings)
        {
            db = dbParam;
            _appSettings = appSettings;


        }


        public ServiceResult<string> wsBasvuruKaydet(IsBasvurusu clsIsBasvuru, long ilanNo)
        {

            try
            {
                Service1Client service1Client = new Service1Client();
                isBasvurusuEkleProxyRequest req = new isBasvurusuEkleProxyRequest();
                req.isBasvurusu = clsIsBasvuru;
                req.ilanNo = ilanNo;
                isBasvurusuEkleProxyResponse resp = service1Client.isBasvurusuEkleProxy(req);
                islemSonucu res = resp.isBasvurusuEkleProxyResult;
                if (res.sonucTuruField == sonucTuru.HATA)
                {
                    return new ServiceResult<string>() { isSuccessfull = true, ResultCode = 1, ResultData = res.sonucTuruField.ToString(), ResultExplanation = res.mesajField.mesaj1Field };
                }
                else {
                    return new ServiceResult<string>() { isSuccessfull = true, ResultCode = 0, ResultData = res.mesajField.mesaj1Field, ResultExplanation = "Başvuru Kaydedildi!" };
                }

              //  return new ServiceResult<string>() { isSuccessfull = true, ResultCode = 0, ResultData = res.mesajField.mesaj1Field, ResultExplanation = "Başvuru Kaydedildi!" };
            }
            catch (Exception Ex)
            {
                return new ServiceResult<string>() { isSuccessfull = false, ResultCode = 1, ResultData = null, ResultExplanation = "Hata Oluştu!"+Ex.Message };
            }
        }
        //TODO
        public ServiceResult<string> wsBasvuruGuncelle(IsBasvurusu clsIsBasvuru)
        {
            return new ServiceResult<string>() { isSuccessfull = true, ResultCode = 0, ResultData = null, ResultExplanation = "Başvuru Güncellendi!" };
        }



        public string getOsymPuan(string sTCNo, string sPuanYil, string sPuanTur)
        {
            TOsym.OSYMSoapClient clsOsymSonuc = new TOsym.OSYMSoapClient( TOsym.OSYMSoapClient.EndpointConfiguration.OSYMSoap);
            TOsym.AuthHeader Authentication = new TOsym.AuthHeader();


            //TOsym.ServisSonucOfArrayOfSinavSonucTemelBilgiYKHEH_S_P5 clsSonucTemelBilgi = new TOsym.ServisSonucOfArrayOfSinavSonucTemelBilgiYKHEH_S_P5();
            //TOsym.ServisSonucOfSonucBilgiXmlYKHEH_S_P5 clsSonucBilgiXML = new TOsym.ServisSonucOfSonucBilgiXmlYKHEH_S_P5();

            TOsym.SonucGetirXmlRequest xmlReq = new TOsym.SonucGetirXmlRequest();

            TOsym.SonucGetirRequest req = new TOsym.SonucGetirRequest();
            req.tcKimlikNo = sTCNo;
            req.sinavYili = int.Parse(sPuanYil);
            req.sinavGrupId = 6;


            Authentication.Username = _appSettings.Nvi_User_Name;
            Authentication.Password = _appSettings.Nvi_Password;
            Authentication.KullaniciBilgisi = "Vakıf Ilan - " + sTCNo;
            Authentication.SessionID = Guid.NewGuid().ToString();
            req.AuthHeader = Authentication;
            // clsOsymSonuc.AuthHeaderValue = Authentication;


            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            bool KpssLisansSonucVarmi = false;
            int KpssLisansSonucId = 0;
            string sPuan = "-1";

            try
            {
               TOsym.SonucGetirResponse clsSonucTemelBilgiResp = clsOsymSonuc.SonucGetir(req);

                if (clsSonucTemelBilgiResp.SonucGetirResult.Sonuc.Length > 0)
                {
                    foreach (var item in clsSonucTemelBilgiResp.SonucGetirResult.Sonuc)
                    {
                        if (item.Ad.IndexOf("KPSS Lisans") >= 0 || item.Ad.IndexOf("KPSS A Grubu ve Öğretmenlik") >= 0)
                        {
                            KpssLisansSonucVarmi = true;
                            KpssLisansSonucId = item.Id;
                            break;
                        }
                    }

                    if (KpssLisansSonucVarmi)
                    {

                        xmlReq.AuthHeader = Authentication;
                        xmlReq.sonucId = KpssLisansSonucId;
                        xmlReq.tcKimlikNo = sTCNo;

                      TOsym.SonucGetirXmlResponse   clsSonucBilgiXMLResp = clsOsymSonuc.SonucGetirXml(xmlReq);
                        if (!string.IsNullOrEmpty(clsSonucBilgiXMLResp.SonucGetirXmlResult.Sonuc.Xml) && clsSonucBilgiXMLResp.SonucGetirXmlResult.Sonuc.Xml.Length > 0)
                        {

                            using (System.IO.Stream stream = new System.IO.MemoryStream(clsSonucBilgiXMLResp.SonucGetirXmlResult.Sonuc.Xml.Length))
                            {
                                System.IO.StreamWriter swText = new System.IO.StreamWriter(stream);
                                swText.Write(clsSonucBilgiXMLResp.SonucGetirXmlResult.Sonuc.Xml);
                                swText.Flush();
                                stream.Position = 0;
                                ds.ReadXml(stream);
                            }

                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                    dt = ds.Tables[0];
                            }

                            if (dt.Rows.Count > 0)
                            {

                                sPuan = dt.Rows[0][sPuanTur].ToString();
                                sPuan = sPuan.Replace(".", ",");
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("ÖSYM Puan " + Ex.Message);
            }

            return sPuan;

        }

        public string[] GetAddressCode(string sTCno)
        {
            string[] sAddress = new string[0];

            try
            {
                MernisAdres.TcKimlikNoSorgulaAdresServisSoapClient adresServis = new MernisAdres.TcKimlikNoSorgulaAdresServisSoapClient(
                MernisAdres.TcKimlikNoSorgulaAdresServisSoapClient.EndpointConfiguration.TcKimlikNoSorgulaAdresServisSoap);
                MernisAdres.KimlikNoileAdresSorguKriteri[] adresKisi = new MernisAdres.KimlikNoileAdresSorguKriteri[1];
                //MernisAdres.KisiAdresBilgileriSonucu adresSonuc = new MernisAdres.KisiAdresBilgileriSonucu();
              //  MernisAdres.KimlikNoileKisiAdresBilgileriSonucu adresSonuc = new MernisAdres.KimlikNoileKisiAdresBilgileriSonucu();

                long TCLong = long.Parse(sTCno);
               // adresServis.Timeout = 10000;

                adresKisi[0] = new MernisAdres.KimlikNoileAdresSorguKriteri();
                adresKisi[0].KimlikNo = TCLong;
                MernisAdres.AuthHeader clsHeader = new MernisAdres.AuthHeader();
                clsHeader.Username = _appSettings.Nvi_User_Name; ;// System.Configuration.ConfigurationManager.AppSettings["Nvi_User_Name"]; //"sguser";
                clsHeader.Password = _appSettings.Nvi_Password;// System.Configuration.ConfigurationManager.AppSettings["Nvi_Password"]; //"sgpwd";
                clsHeader.SorgulayanKullanici = "Vakıf Ilan - " + sTCno;
                clsHeader.SessionID = Guid.NewGuid().ToString();
                //  adresServis.AuthHeaderValue = clsHeader;

                MernisAdres.SorgulaRequest req = new MernisAdres.SorgulaRequest()
                {
                    AuthHeader = clsHeader,
                    Kriter = adresKisi
                };


               MernisAdres.SorgulaResponse adresSonucResp = adresServis.Sorgula(req);

                if (adresSonucResp != null)
                {
                    if (adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi != null)
                    {
                        sAddress = new string[4];

                        string YerlesimYeriAdresTipi = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.AdresTip.Kod.ToString();

                        if (YerlesimYeriAdresTipi == "1")//IlIlceMerkezAdresi
                        {
                            sAddress[0] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.IlIlceMerkezAdresi.Ilce.ToString();
                            sAddress[1] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.IlIlceMerkezAdresi.IlceKodu.ToString();
                            sAddress[2] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.IlIlceMerkezAdresi.Il.ToString();
                            sAddress[3] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.IlIlceMerkezAdresi.IlKodu.ToString();
                        }
                        else if (YerlesimYeriAdresTipi == "2")//Belde
                        {
                            sAddress[0] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.BeldeAdresi.Ilce.ToString();
                            sAddress[1] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.BeldeAdresi.IlceKodu.ToString();
                            sAddress[2] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.BeldeAdresi.Il.ToString();
                            sAddress[3] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.BeldeAdresi.IlKodu.ToString();
                        }
                        else if (YerlesimYeriAdresTipi == "3")//Köy
                        {
                            sAddress[0] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.KoyAdresi.Ilce.ToString();
                            sAddress[1] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.KoyAdresi.IlceKodu.ToString();
                            sAddress[2] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.KoyAdresi.Il.ToString();
                            sAddress[3] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.KoyAdresi.IlKodu.ToString();
                        }
                        else if (YerlesimYeriAdresTipi == "4")//Yurt Dışı
                        {
                            sAddress[0] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.YurtDisiAdresi.YabanciUlke.Aciklama.ToString();
                            sAddress[1] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.YurtDisiAdresi.YabanciUlke.Kod.ToString();
                            sAddress[2] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.YurtDisiAdresi.YabanciSehir.ToString();
                            sAddress[3] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.YurtDisiAdresi.ToString();
                        }
                        else
                        {
                            sAddress[0] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.IlIlceMerkezAdresi.Ilce.ToString();
                            sAddress[1] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.IlIlceMerkezAdresi.IlceKodu.ToString();
                            sAddress[2] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.IlIlceMerkezAdresi.Il.ToString();
                            sAddress[3] = adresSonucResp.SorgulaResult.SorguSonucu[0].YerlesimYeriAdresi.IlIlceMerkezAdresi.IlKodu.ToString();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //  Status.InnerHtml = Ex.Message;
                sAddress[0] = "0";
                sAddress[1] = Ex.Message;
            }

            return sAddress;
        }

      

        public MernisSorguSonuc CuzdanKontrol(SignupModel model, string sTCno, string SeriNo, bool yeniKimlikSahibi)
        {
            MernisSorguSonuc mernisSorguSonuc = null;

            BilesikKutukServis.BilesikKutukSorgulaKimlikNoServiceSoapClient client = new BilesikKutukServis.BilesikKutukSorgulaKimlikNoServiceSoapClient
                 (BilesikKutukServis.BilesikKutukSorgulaKimlikNoServiceSoapClient.EndpointConfiguration.BilesikKutukSorgulaKimlikNoServiceSoap);

            //BilesikKutukServis.BilesikKutukSorgulaKimlikNoServiceSoap  clsService = new BilesikKutukServis.BilesikKutukSorgulaKimlikNoServiceSoap();

            BilesikKutukServis.BilesikKutukSorgulaKimlikNoSorguKriteri[] tcKriter = new BilesikKutukServis.BilesikKutukSorgulaKimlikNoSorguKriteri[1];
            BilesikKutukServis.BilesikKutukSorgulaKimlikNoSorguKriteri tc = new BilesikKutukServis.BilesikKutukSorgulaKimlikNoSorguKriteri();
            //BilesikKutukServis.BilesikKutukBilgileriSonucu sonuc = new BilesikKutukServis.BilesikKutukBilgileriSonucu();
            BilesikKutukServis.TCCuzdanBilgisi cuzdanBilgisi = new BilesikKutukServis.TCCuzdanBilgisi();
            BilesikKutukServis.TCKK cuzdanKayitBilgisi = new BilesikKutukServis.TCKK();


            BilesikKutukServis.AuthHeader clsHeader = new BilesikKutukServis.AuthHeader();
            clsHeader.Username = _appSettings.Nvi_User_Name;// "VakifIlan";// System.Configuration.ConfigurationManager.AppSettings["Nvi_User_Name"]; //"sguser";
            clsHeader.Password = _appSettings.Nvi_Password;// System.Configuration.ConfigurationManager.AppSettings["Nvi_Password"]; //"sgpwd";
            clsHeader.SorgulayanKullanici = "Vakıf Ilan - " + sTCno;
            clsHeader.SessionID = Guid.NewGuid().ToString();
            System.Globalization.CultureInfo clsCulture = new System.Globalization.CultureInfo("tr-TR");

            #region ws call


            int iStatus = 1;
            //iStatus   0 TC Yok
            //iStatus   1 TC ve Kimlik Bilgileri Doğru
            //iStatus   2 Kimlik Bilgileri Yanlış

            if (Util.TcKontrol(sTCno))
            {
                try
                {
                    long TCLong = long.Parse(model.tckn);
                    tc.KimlikNo = TCLong;
                    tcKriter[0] = tc;
                    //clsservice. AuthHeaderValue = clsHeader;
                    //sonuc = clsService.BilesikKutukSorgulaKimlikNoServis(tcKriter);

                    var resp = client.BilesikKutukSorgulaKimlikNoServisAsync(clsHeader, tcKriter).Result;

                    if (resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu != null && resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu.Length > 0)
                    {
                        if (resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].HataBilgisi == null)
                        {
                            if (resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.KisiBilgisi.HataBilgisi == null)
                            {
                                if (yeniKimlikSahibi)
                                {
                                    cuzdanKayitBilgisi = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.TCKKBilgisi;


                                    //Yeni cüzdan alanlara geçici kimlik veriliyor. Geçici kimlikmi var onu kontrol ediyoruz.                                         
                                    if (!string.IsNullOrEmpty(cuzdanKayitBilgisi.SeriNo) && !string.IsNullOrEmpty(cuzdanKayitBilgisi.Ad) && !string.IsNullOrEmpty(cuzdanKayitBilgisi.Soyad))
                                    {
                                        if (cuzdanKayitBilgisi.Ad.ToUpper(clsCulture) == model.name.ToUpper(clsCulture).Trim() &&
                                             cuzdanKayitBilgisi.Soyad.ToUpper(clsCulture) == model.surname.ToUpper(clsCulture).Trim() &&
                                             cuzdanKayitBilgisi.SeriNo == SeriNo.Trim())
                                        {
                                            iStatus = 1;
                                            BilesikKutukServis.TarihBilgisi dDogumTarihi = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.DogumTarih;
                                            mernisSorguSonuc = new MernisSorguSonuc()
                                            {
                                                MernisAd = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.Ad,
                                                MernisSoyad = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.Soyad,
                                                MernisCinsiyet = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.Cinsiyet.Aciklama,
                                                MernisDogumTarihi = Convert.ToDateTime((dDogumTarihi.Gun < 10 ? "0" : "") + dDogumTarihi.Gun.ToString() + "." + (dDogumTarihi.Ay < 10 ? "0" : "") + dDogumTarihi.Ay.ToString() + "." + dDogumTarihi.Yil.ToString()).ToShortDateString(),
                                                MernisDogumYeri = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.KisiBilgisi.TemelBilgisi.DogumYer,
                                                MernisMedeniHal = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.KisiBilgisi.DurumBilgisi.MedeniHal.Aciklama,
                                                TCKimlikNo = sTCno,
                                                SonucKodu = 1
                                            };

                                        }
                                        else
                                        {
                                            PersonelInfo personelInfo = new PersonelInfo()
                                            {
                                                TCKimlikNo = TCLong,
                                                MernisAd = cuzdanKayitBilgisi.Ad.ToUpper(clsCulture),
                                                MernisSoyad = cuzdanKayitBilgisi.Soyad.ToUpper(clsCulture),
                                                MernisSeriNo = cuzdanKayitBilgisi.SeriNo,
                                                BasvuranAd = model.name.ToUpper(clsCulture).Trim(),
                                                BasvuranSoyad = model.surname.ToUpper(clsCulture).Trim(),
                                                BasvuranSeriNo = SeriNo.Trim(),
                                                LogType = "0"//Yeni kimlik sahibi
                                            };

                                            db.AddPersonelInfoLog(personelInfo);

                                            iStatus = 2;
                                            mernisSorguSonuc = new MernisSorguSonuc() { SonucKodu = 2 };
                                        }
                                    }
                                    else
                                    {

                                        /** Geçici kimlik sahibi  */

                                        if ((!string.IsNullOrEmpty(resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.GeciciKimlikBilgisi.TCKimlikNo.ToString())
                                             && resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.GeciciKimlikBilgisi.TCKimlikNo == TCLong)
                                             && (!string.IsNullOrEmpty(resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.GeciciKimlikBilgisi.Ad))
                                             && resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.GeciciKimlikBilgisi.Ad.ToUpper() == model.name.ToUpper().Trim()
                                             && (!string.IsNullOrEmpty(resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.GeciciKimlikBilgisi.Soyad)
                                             && resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.GeciciKimlikBilgisi.Soyad.ToUpper() == model.surname.ToUpper().Trim()))
                                        {
                                            mernisSorguSonuc = new MernisSorguSonuc() { SonucKodu = 1 };
                                            // iStatus = 1;
                                        }
                                        else
                                        {
                                            PersonelInfo personelInfo = new PersonelInfo()
                                            {
                                                TCKimlikNo = TCLong,
                                                MernisAd = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.GeciciKimlikBilgisi.Ad.ToUpper(),
                                                MernisSoyad = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.GeciciKimlikBilgisi.Soyad.ToUpper(),
                                                MernisSeriNo = "",
                                                BasvuranAd = model.name.ToUpper().Trim(),
                                                BasvuranSoyad = model.surname.ToUpper().Trim(),
                                                BasvuranSeriNo = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.GeciciKimlikBilgisi.TCKimlikNo.ToString().Trim(),
                                                LogType = "2"//Geçici kimlik sahibi 
                                            };

                                            db.AddPersonelInfoLog(personelInfo);
                                            mernisSorguSonuc = new MernisSorguSonuc() { SonucKodu = 2 };
                                            //iStatus = 2;
                                        }
                                    }
                                }
                                else
                                {
                                    cuzdanBilgisi = resp.BilesikKutukSorgulaKimlikNoServisResult.SorguSonucu[0].TCVatandasiKisiKutukleri.NufusCuzdaniBilgisi;
                                    string cuzdanSeriNo = cuzdanBilgisi.Seri + cuzdanBilgisi.No.ToString();
                                    if (cuzdanBilgisi.Ad.ToUpper(clsCulture) == model.name.ToUpper(clsCulture).Trim()
                                         && cuzdanBilgisi.Soyad.ToUpper(clsCulture) == model.surname.ToUpper(clsCulture).Trim()
                                         && cuzdanSeriNo == SeriNo)
                                    {
                                        mernisSorguSonuc = new MernisSorguSonuc() { SonucKodu = 1 };
                                        //iStatus = 1;
                                    }
                                    else
                                    {
                                        PersonelInfo personelInfo = new PersonelInfo()
                                        {
                                            TCKimlikNo = TCLong,
                                            MernisAd = cuzdanBilgisi.Ad.ToUpper(clsCulture),
                                            MernisSoyad = cuzdanBilgisi.Soyad.ToUpper(clsCulture),
                                            MernisSeriNo = cuzdanSeriNo,
                                            BasvuranAd = model.name.ToUpper().Trim(),
                                            BasvuranSoyad = model.surname.ToUpper().Trim(),
                                            BasvuranSeriNo = SeriNo,
                                            LogType = "1"//Eski tip cüzdan
                                        };

                                        db.AddPersonelInfoLog(personelInfo);
                                        mernisSorguSonuc = new MernisSorguSonuc() { SonucKodu = 2 };
                                        //iStatus = 2;
                                    }
                                }
                            }
                            else
                            {
                                mernisSorguSonuc = new MernisSorguSonuc() { SonucKodu = 0 };
                                //iStatus = 0;
                            }
                        }
                        else
                        {
                            mernisSorguSonuc = new MernisSorguSonuc() { SonucKodu = 0 };
                            //iStatus = 0;
                        }
                    }
                    else
                    {
                        mernisSorguSonuc = new MernisSorguSonuc() { SonucKodu = 0 };
                        // iStatus = 0;
                    }
                }
                catch (Exception Ex)
                {
                    mernisSorguSonuc = new MernisSorguSonuc() { SonucKodu = 0 };
                    //iStatus = 0;
                }
            }
            else
            {
                mernisSorguSonuc = new MernisSorguSonuc() { SonucKodu = 0 };
                iStatus = 0;
            }

            return mernisSorguSonuc;

            #endregion
        }


        //public void IlanListesi()
        //{

        //    try
        //    {


        //        iseAlimTalebi[] clsDizi = new iseAlimTalebi[0];
        //        iseAlimTalebi clsTalep = new iseAlimTalebi();
        //        ilBilgisi clsIlBilgisi = new ilBilgisi();
        //        ilceBilgisi clsIlceBilgisi = new ilceBilgisi();
        //        clsIlBilgisi.mernisIlKoduSpecified = true;

        //        string Username = "webservis";
        //        string Password = "weblogic";// System.Configuration.ConfigurationManager.AppSettings["btnInfo2"];
        //        System.Net.NetworkCredential netCred = new System.Net.NetworkCredential(Username, Password);
        //        System.ServiceModel.Description.ClientCredentials netCredNew = new System.ServiceModel.Description.ClientCredentials();

        //        //netCredNew.UserName = Username;
        //        // JobAnnouncementWebServiceManagerClient clsService = new JobAnnouncementWebServiceManagerClient();


        //        JobAnnouncementWebServiceManagerClient clsService = new JobAnnouncementWebServiceManagerClient(JobAnnouncementWebServiceManagerClient.EndpointConfiguration.JobAnnouncementWebServiceManagerPort);
        //        //  clsService.ClientCredentials.Windows.ClientCredential = netCred;
        //        //clsService.GetHas


        //        //ClientCredentials cred = clsService.ChannelFactory.Endpoint.EndpointBehaviors[0] as ClientCredentials;

        //        //cred.HttpDigest.ClientCredential.UserName = "webservis";
        //        //cred.HttpDigest.ClientCredential.Password = "weblogic";
        //        //cred.HttpDigest.ClientCredential.Domain = "aspbtest.ailevecalisma.gov.tr";

        //        // clsService.ChannelFactory.Endpoint.EndpointBehaviors[0] = cred;

        //        // clsService.ChannelFactory.Endpoint.EndpointBehaviors[0].ApplyClientBehavior(cred);

        //        // clsService.

        //        //credentialBehaviour.UserName.UserName = "test";
        //        //credentialBehaviour.UserName.Password = "test";

        //        //var defaultCredentials = clsService.Endpoint.Behaviors.Find<ClientCredentials>();
        //        //factory.Endpoint.Behaviors.Remove(defaultCredentials);

        //        clsService.ChannelFactory.Credentials.UserName.UserName = "webservis";
        //        clsService.ChannelFactory.Credentials.UserName.Password = "weblogic";

        //        //clsService.Endpoint.EndpointBehaviors.Add( = net
        //        //clsService.PreAuthenticate = true;

        //      //  var rsp = clsService.iseAlimTalepleriniSorgulaAsync(0, clsIlBilgisi, clsIlceBilgisi, unvan.VAKIF_MUDURU, DateTime.Now, DateTime.Now, true).Result;


        //    }
        //    catch (Exception e)
        //    {

        //        string msg = e.Message;

        //    }


        //}

        public IseAlimTalebi[] IlanListesi()
        {




            //iseAlimTalebi[] clsDizi = new iseAlimTalebi[0];
            //iseAlimTalebi clsTalep = new iseAlimTalebi();
            //ilBilgisi clsIlBilgisi = new ilBilgisi();
            //ilceBilgisi clsIlceBilgisi = new ilceBilgisi();
            // clsIlBilgisi.mernisIlKoduSpecified = true;          

            //netCredNew.UserName = Username;
            Service1Client service1Client = new Service1Client();
            GetDataRequest req = new GetDataRequest();
            GetDataResponse resp = service1Client.GetData(req);

            // resp.GetDataResult[0].calismaSekliField;

            iseAlimTalebi[] resultArr = resp.GetDataResult;
            List<IseAlimTalebi> finArr = new List<IseAlimTalebi>();

            for (int c = 0; c < resultArr.Length; c++)
            {
                if (Util.DateBetween(resultArr[c].basvuruBaslangicZamaniField, resultArr[c].basvuruBitisZamaniField))
                {

                    IseAlimTalebi newTalep =  new IseAlimTalebi();
                   newTalep.Visibility = false;
                   newTalep.iseAlimTalebiNoField = resultArr[c].iseAlimTalebiNoField;
                   newTalep.districtField = resultArr[c].districtField.ilceAdiField;
                   newTalep.IlanIlceID = resultArr[c].districtField.mernisIlceKoduField.ToString();
                   newTalep.cityField = resultArr[c].cityField.ilAdiField;
                   newTalep.IlanIlID = resultArr[c].cityField.mernisIlKoduField.ToString();

                   newTalep.unvanField = Util.ReplaceText(resultArr[c].unvanField.ToString());

                   newTalep.baslikField = resultArr[c].baslikField;
                   newTalep.basvuruBitisZamaniField = resultArr[c].basvuruBitisZamaniField.ToShortDateString();
                   newTalep.belgeTeslimTarihiField = resultArr[c].belgeTeslimTarihiField.ToShortDateString();
                   newTalep.calismaSekliField = Util.ReplaceText(resultArr[c].calismaSekliField.ToString());

                   newTalep.bilgisayarBilgisiGerekliField = resultArr[c].bilgisayarBilgisiGerekliField;
                   newTalep.bolumGerekliField = resultArr[c].bolumGerekliField;//true
                   newTalep.dilGerekliField = resultArr[c].dilGerekliField;//true
                   newTalep.surucuBelgesiGerekliField = resultArr[c].surucuBelgesiGerekliField;

                   newTalep.bilgisayarBilgisiAciklamasiField = resultArr[c].bilgisayarBilgisiAciklamasiField;
                   newTalep.bolumAciklamasiField = resultArr[c].bolumAciklamasiField;//"Bilgisayar Müh..";
                   newTalep.dilAciklamasiField = resultArr[c].dilAciklamasiField;//"İngileizce bir zahmet";//
                   newTalep.surucuBelgesiAciklamasiField = resultArr[c].surucuBelgesiAciklamasiField;

                    if (resultArr[c].ilGerekliField)
                       newTalep.ilGerekliField = "Aday vakfın bulunduğu il sınırları içinde ikamet ediyor olmalıdır!";
                    if (resultArr[c].ilceGerekliField)
                       newTalep.ilceGerekliField = "Aday vakfın bulunduğu ilçe sınırları içinde ikamet ediyor olmalıdır!";

                   newTalep.aciklamaField = @"Bilgisayar ve Microsoft Office Programlarını iyi derecede kullanabiliyor olmak
                - Türkiye sınırlarında örgün eğitim veren üniversitelerin 4 yıllık Sosyal Hizmetler Bölümünden mezun olmak.
                - En az B sınıfı ehliyet sahibi olmak ve aktif olarak araç kullanabiliyor olmak
                - Aday vakfın bulunduğu ilçe sınırları içinde ikamet ediyor olmalı
                - Adayların Kartal İlçesinde 5 yıldır ikamet ediyor olmak,
                1) Türkiye Cumhuriyeti vatandaşı olmak, 
                2) Medeni haklarını kullanma ehliyetine sahip olmak, 
                3) 18 yaşını bitirmiş olmak ve 35 yaşını doldurmamış olmak, 
                4) Askerlik görevini yapmış veya tecil ettirmiş olmak,
                5) Kamu haklarından mahrum bulunmamak, 
                6) Türk Ceza Kanununun 53 üncü maddesinde belirtilen süreler geçmiş olsa bile; kasten işlenen bir suçtan dolayı bir yıl veya daha fazla süreyle 
                    hapis cezasına ya da affa uğramış olsa bile devletin güvenliğine karşı suçlar, Anayasal düzene ve bu düzenin işleyişine karşı suçlar, 
                    zimmet, irtikap, rüşvet, hırsızlık, dolandırıcılık, sahtecilik, güveni kötüye kullanma, hileli iflas, ihaleye fesat karıştırma, edimin ifasına fesat karıştırma, 
                    suçtan kaynaklanan malvarlığı değerlerini aklama veya kaçakçılık suçlarından mahkum olmamak. 7) Vakıfta ilk defa istihdam edilecek Sosyal Yardım ve İnceleme Görevlisinde, 
                    4 Yıllık Yükseköğretim Kurumlarının belirtilen bölümden mezun olmak ve ÖSYM tarafından yapılan geçerli Kamu Personeli Seçme Sınavında 2018-2019 KPSSP3 puan türünde en az 60 puan almış olmak
                    şartları aranır8) Erkek olmak. 9 )Görevini devamlı yapmasına engel olabilecek akli ya da bedensel engeli bulunmamak.10)
                    Başvurular şahsen yapılacak olup posta ile yapılan başvurular kabul edilmeyecektir.Personel alımını yapıp yapmamakta Kartal Sosyal Yardımlaşma ve Dayanışma Vakfı yetkilidir.
                    *İşe alınacak personel sayısının 5 katı kadar aday geçerli KPSS P3 puanına göre en yüksek puandan başlayarak sıralanır ve mülakata çağrılır";

                    finArr.Add(newTalep);
                    //   newTalep.aciklamaField = resultArr[c].aciklamaField;
                }
            }

            return finArr.ToArray();

        }






        //public MernisSorguSonuc MernistenSorgula(string sTCno)
        //{
        //    System.Globalization.CultureInfo clsCulture = new System.Globalization.CultureInfo("tr-TR");

        //    try
        //    {
        //        MernisTc.NkoSorgulaTCKimlikNoServisSoapClient nkoServis = new MernisTc.NkoSorgulaTCKimlikNoServisSoapClient(
        //            MernisTc.NkoSorgulaTCKimlikNoServisSoapClient.EndpointConfiguration.NkoSorgulaTCKimlikNoServisSoap);


        //        MernisTc.NkoTCKimlikNoSorguKriteri[] nkoKisi = new MernisTc.NkoTCKimlikNoSorguKriteri[1];
        //      //  NkoSonucu kisiSonuc = new NkoSonucu();
        //        long TCLong = long.Parse(sTCno);

        //       // nkoServis.Timeout = 10000;

        //        nkoKisi[0] = new NkoTCKimlikNoSorguKriteri();
        //        nkoKisi[0].EskiEsListele = false;
        //        //nkoKisi[0].EskiEsListeleSpecified = true;
        //        nkoKisi[0].NKOTipi = NkoTur.KisiKayitOrnek;
        //        //nkoKisi[0].NKOTipiSpecified = true;
        //        nkoKisi[0].TCKimlikNo = TCLong;
        //        //nkoKisi[0].TCKimlikNoSpecified = true;
        //        nkoKisi[0].Vukuatli = false;
        //        nkoKisi[0].Vukuatli = true;

        //        MernisTc.AuthHeader clsHeader = new MernisTc.AuthHeader();
        //        clsHeader.Username = "VakifIlan";// System.Configuration.ConfigurationManager.AppSettings["Nvi_User_Name"]; //"sguser";
        //        clsHeader.Password = "JETAp#2vuzU$";// System.Configuration.ConfigurationManager.AppSettings["Nvi_Password"]; //"sgpwd";
        //        clsHeader.SorgulayanKullanici = "Vakıf Ilan - " + sTCno;
        //        clsHeader.SessionID = Guid.NewGuid().ToString();
        //       // nkoServis.AuthHeaderValue = clsHeader;

        //       var kisiSonuc = nkoServis.SorgulaAsync(clsHeader,nkoKisi).Result;

        //        if (kisiSonuc != null)
        //        {
        //            NkoKisi kisi = new NkoKisi();
        //            try
        //            {
        //                kisi = kisiSonuc.SorgulaResult.SorguSonucu[0].Kisiler[0];
        //            }
        //            catch (Exception Ex)
        //            {
        //                throw new Exception(Ex.Message);
        //            }

        //         if (kisi != null)
        //            {
        //                string sTCKimlikNo = kisi.TCKimlikNo.Value.ToString();
        //                string sAd = kisi.TemelBilgisi.Ad.ToUpper(clsCulture);
        //                string sSoyad = kisi.TemelBilgisi.Soyad.ToUpper(clsCulture);
        //                MernisTc.TarihBilgisi dDogumTarihi = kisi.TemelBilgisi.DogumTarih;
        //                string sDogumYeri = kisi.TemelBilgisi.DogumYer.ToUpper(clsCulture);
        //                string sIl = kisi.KayitYeriBilgisi.Il.Kod.Value.ToString();
        //                string sIlce = kisi.KayitYeriBilgisi.Ilce.Kod.Value.ToString();
        //                int iCilt = kisi.KayitYeriBilgisi.Cilt.Kod.Value;
        //                int iAileSira = kisi.KayitYeriBilgisi.AileSiraNo.Value;
        //                int iSira = kisi.KayitYeriBilgisi.BireySiraNo.Value;
        //                string sCinsiyet = kisi.TemelBilgisi.Cinsiyet.Aciklama.ToString();
        //                string sMedeniHal = kisi.DurumBilgisi.MedeniHal.Aciklama.ToString().ToUpper(new System.Globalization.CultureInfo("en-US"));

        //                //hdnDogumYeri.Value = sDogumYeri;
        //                //hdnCinsiyet.Value = sCinsiyet;
        //                //hdnMedeniDurum.Value = sMedeniHal;

        //                string sDogumTarih = ""; // Convert.ToDateTime((dDogumTarihi.Gun < 10 ? "0" : "") + dDogumTarihi.Gun.ToString() + "." + (dDogumTarihi.Ay < 10 ? "0" : "") + dDogumTarihi.Ay.ToString() + "." + dDogumTarihi.Yil.ToString()).ToShortDateString();
        //                try
        //                {
        //                    sDogumTarih = Convert.ToDateTime((dDogumTarihi.Gun < 10 ? "0" : "") + dDogumTarihi.Gun.ToString() + "." + (dDogumTarihi.Ay < 10 ? "0" : "") + dDogumTarihi.Ay.ToString() + "." + dDogumTarihi.Yil.ToString()).ToShortDateString();
        //                }
        //                catch {
        //                    throw new Exception("Doğum Tarihi Doğrulanamadı!");
        //                }

        //                return new MernisSorguSonuc { MernisDogumTarihi = sDogumTarih, MernisCinsiyet= sCinsiyet, MernisMedeniHal = sMedeniHal, MernisDogumYeri=sDogumYeri };
        //            }
        //            else
        //            {
        //                throw new Exception("Kişi Mernisten Bulunamadı!");
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Kişi Mernisten Bulunamadı!");
        //        }
        //    }
        //    catch (Exception Ex)
        //    {

        //        throw new Exception(Ex.Message);
        //    }
        //}

        public string SendMail(SignupModel model)
        {
            string sResult = "";

            string sContent = "<br/><br/>";

            sContent += "Sayın " + model.name + " " + model.surname;

            if (model.password != "")
            {
                sContent += "<br/><br/><b>Şifreniz :</b> " + model.password;
            }

            sContent += "<br/><br/><br/><br/><br/>";
            sContent += "Başvuru yapmak için Tc Kimlik No ve şifrenizi kullanarak giriş yapabilirsiniz.";
            sContent += "<br/><br/><br/><br/><br/><br/>AİLE, ÇALIŞMA ve SOSYAL HİZMETLER BAKANLIĞI<br/>";
            sContent += "<br /><br /><br />";

            string sPath = ("./images/Mesaj_alti.png"); // my logo is placed in images folder

            sContent += "<img src='" + sPath + "'/>";
            string sHost = "", sGonderen = "", sUserName = "", sUserPassword = "";

            try
            {
                sGonderen = _appSettings.Mail_Gonderen;

                sUserName = _appSettings.Mail_User_Name;
                // sUserName = StringCipher.Decrypt(username, passPhrase);

                sUserPassword = _appSettings.Mail_User_Password;
                //sUserPassword = StringCipher.Decrypt(password, passPhrase);
                sHost = _appSettings.Mail_Host;
            }
            catch { }
            sResult = clsMail.SendMail(sGonderen, model.email, "Vakıf İş Başvurusu Şifre", sContent, sHost, sUserName, sUserPassword, 25);
            return sResult;
        }

        //private void InsertLog(string sError)
        //{


        //    var sonuc = db.AddPersonelError(Session["ID"].ToString(),
        //        cmbIl.SelectedValue, cmbIlce.SelectedValue,
        //        cmbUnvan.SelectedValue, cmbIseAlimTalebi.SelectedValue,
        //        Session["BasvuruNo"].ToString(), sError, sIP);
        //    if (!sonuc.BasariliMi)
        //    {
        //        if (sonuc.HataBilgi != null)
        //        {
        //            db.AddPersonelSqlLog(Session["ID"].ToString(), "Personel Error -" + sonuc.HataBilgi.HataMesaj, sIP);
        //        }

        //    }
        //}
        //private void UpdatePersonData(string[] sAddress)
        //{


        //    var sonuc = db.SetPersonData(Session["ID"].ToString(),
        //        HttpUtility.HtmlEncode(cmbAskerlik.SelectedValue),
        //        HttpUtility.HtmlEncode(cmbEgitimDurumu.SelectedValue),
        //        Convert.ToDateTime(HttpUtility.HtmlEncode(txtMezuniyetTarihi.Value)).ToString("dd/MM/yyyy"),
        //        HttpUtility.HtmlEncode(txtBolum.Text),
        //        HttpUtility.HtmlEncode(cmbKPSSYili.SelectedValue),
        //        HttpUtility.HtmlEncode(txtKPSSPuan.Text.Replace(".", ",")),
        //        HttpUtility.HtmlEncode(txtEvTel.Text),
        //        HttpUtility.HtmlEncode(txtIsTelefonu.Text),
        //        HttpUtility.HtmlEncode(txtCepTelefonu.Text),
        //        HttpUtility.HtmlEncode(txtEPosta.Text),
        //        sIP,
        //        HttpUtility.HtmlEncode(txtTecilTarihi.Value),
        //        sAddress);
        //    if (sonuc.BasariliMi)
        //    {
        //        if (Session["BasvuruNo"].ToString() != "")
        //        {
        //            db.SetPersonelBasvuru(Session["ID"].ToString(), Session["IlanNo"].ToString(), Session["BasvuruNo"].ToString());
        //        }
        //    }
        //    else
        //    {
        //        if (sonuc.HataBilgi != null)
        //        {
        //            db.AddPersonelSqlLog(Session["ID"].ToString(), sonuc.HataBilgi.HataMesaj, sIP);
        //        }
        //    }
        //}
    }

}
