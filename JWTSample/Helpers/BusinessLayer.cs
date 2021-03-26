using JobWcfService;
using JWTSample.AuxClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VakifIlan;

namespace JWTSample.Helpers
{
    public class BusinessLayer
    {
        VakifDb db;
        AppSettings _appSettings;
        Mail clsMail = new Mail();
        IsBasvurusuBilgileri basvuru;
        WSHelper wsHelper;
        public BusinessLayer(VakifDb dbParam, AppSettings appSettings, IsBasvurusuBilgileri pBasvuru)
        {
            db = dbParam;
            _appSettings = appSettings;
            basvuru = pBasvuru;
            wsHelper = new WSHelper(db, _appSettings);
        }


        public ServiceResult<string[]> checkAdresCode()
        {
            string[] sAddressArr = wsHelper.GetAddressCode(basvuru.TcKimlikNo.ToString());
            if (sAddressArr.Length > 1 && sAddressArr[0] != "0")
            {
                return new ServiceResult<string[]>()
                {
                    isSuccessfull = true,
                    ResultCode = 0,
                    ResultData = sAddressArr,
                    ResultExplanation = "Adres Bilgisi Alındı"
                };
            }

            else
            {
                return new ServiceResult<string[]>()
                {
                    isSuccessfull = false,
                    ResultCode = 1,
                    ResultData = null,
                    ResultExplanation = "Sistemden adres bilginize ulaşılamamıştır.</br> İkametgah şartı isteyen ilanlara başvurabilmek için nüfus müdürlüğüne gidip adres kaydınızı yaptırmanız gerekmektedir."
                };
            }
        }


        public ServiceResult<string> checkOsymPuan()
        {


            string sKpssPuan = "0";
            try
            {

                string[] sAddress = new string[0];
                try
                {
                    sKpssPuan = wsHelper.getOsymPuanWsIntra(basvuru.TcKimlikNo, basvuru.KpssGirisYili.ToString(), "PUAN_3");
                }
                catch (Exception exc)
                {
                    return new ServiceResult<string>()
                    {
                        isSuccessfull = false,
                        ResultCode = 1,
                        ResultData = string.Empty,
                        ResultExplanation = "ÖSYM Puan sorgulamada sistem kaynaklı hata oluştu!Yardım Masasına bilgi veriniz" + exc.Message
                    };

                }

                if (sKpssPuan == "-1")
                {
                    return new ServiceResult<string>()
                    {
                        isSuccessfull = false,
                        ResultCode = 1,
                        ResultData = null,
                        ResultExplanation = "Girilen yıla ait KPSS Puanı bulunamadı.<br/>Lütfen tekrar deneyiniz."
                    };

                }
                else if (!(sKpssPuan == basvuru.KpssPuani.ToString().Replace('.', ',')))
                {
                    return new ServiceResult<string>()
                    {
                        isSuccessfull = false,
                        ResultCode = 1,
                        ResultData = null,
                        ResultExplanation = "Girilen KPSS Puanı ile ÖSYM den dönen KPSS Puanı aynı değil.Lütfen tekrar deneyiniz."

                    };
                }
                else
                {
                    string sPuan = sKpssPuan.Replace(".", ",");
                    if (Convert.ToDouble(sPuan) < 60)
                    {

                        return new ServiceResult<string>()
                        {
                            isSuccessfull = false,
                            ResultCode = 1,
                            ResultData = null,
                            ResultExplanation = "Girilen yıla ait KPSS Puanınız 60 dan küçük.<br/>Bu ünvana başvurmak için puanınız 60 dan büyük olmalı"
                        };
                    }
                }
            }
            catch (Exception)
            {
                return new ServiceResult<string>()
                {
                    isSuccessfull = false,
                    ResultCode = 1,
                    ResultData = null,
                    ResultExplanation = "Beklenmeyen Hata Oluştu!"
                };
            }

            return new ServiceResult<string>()
            {
                isSuccessfull = true,
                ResultCode = 0,
                ResultData = sKpssPuan,
                ResultExplanation = "Puan Kontrolü Başarılı"
            };
        }


        public void UpdatePersonData(IsBasvurusuBilgileri basvuru, string[] sAddress)
        {
            var sonuc = db.SetPersonData(basvuru.PersonelID,
              basvuru.AskerlikDurumu,
              basvuru.EgitimDurumu,
              basvuru.MezuniyetTarihi.ToString(),
              basvuru.UniversiteBolum,
              basvuru.KpssGirisYili.ToString(),
             basvuru.KpssPuani.ToString(),//  txtKPSSPuan.Text.Replace(".", ",")),
             basvuru.EvTelNumarasi,//  txtEvTel.Text),
             basvuru.IsTelNumarasi,
             basvuru.CepTelefonu,
             basvuru.EPosta,
             "127.0.0.1",
             basvuru.TecilTarihi.ToString(),
             sAddress);
            //if (sonuc.BasariliMi)
            //{
            //    if (basvuru.BasvuruNo != "" && basvuru.BasvuruNo != "0")
            //    {
            //        db.SetPersonelBasvuru(basvuru.PersonelID, basvuru.IlanID.ToString(), basvuru.BasvuruNo);
            //    }
            //}
            //else
            //{
            //    if (sonuc.HataBilgi != null)
            //    {
            //        db.AddPersonelSqlLog(basvuru.PersonelID, sonuc.HataBilgi.HataMesaj, "127.0.0.1");
            //    }
            //}
        }

        //private void InsertLog(IsBasvurusuBilgileri basvuru, string sError)
        //{
        //    var sonuc = db.AddPersonelError(basvuru.PersonelID,
        //       basvuru.IlanIlID,
        //        basvuru.IlanIlceID, //cmbIlce.SelectedValue,
        //        basvuru.UnvanID,
        //        basvuru.IlanID.ToString(),              
        //        basvuru.BasvuruNo,
        //        sError,
        //        basvuru.Ip);
        //    if (!sonuc.BasariliMi)
        //    {
        //        if (sonuc.HataBilgi != null)
        //        {
        //            db.AddPersonelSqlLog(basvuru.PersonelID, "Personel Error -" + sonuc.HataBilgi.HataMesaj, basvuru.Ip);
        //        }

        //    }
        //}


        //private void InsertBasvuru(IsBasvurusuBilgileri basvuru)
        //{
        //    var sonuc = db.AddPersonelBasvuru(basvuru.PersonelID,
        //         basvuru.IlanIlID,
        //        basvuru.IlanIlceID,
        //          basvuru.UnvanID,
        //         basvuru.IlanID.ToString(),
        //       basvuru.Ip);
        //    if (!sonuc.BasariliMi)
        //    {
        //        if (sonuc.HataBilgi != null)
        //        {
        //            db.AddPersonelSqlLog(basvuru.PersonelID, "Personel Basvuru -" + sonuc.HataBilgi.HataMesaj, basvuru.Ip);
        //        }
        //    }
        //}



        public ServiceResult<string> basvuruKaydet(IsBasvurusu clsIsBasvuru, long ilanNo)
        {
            try
            {
             ServiceResult<string> result  = wsHelper.wsBasvuruKaydet(clsIsBasvuru,ilanNo);
                return result;
            }
            catch (Exception Ex)
            {
                return new ServiceResult<string>() { isSuccessfull = false, ResultCode = 0, ResultData = null, ResultExplanation = "Başvuru Kaydedilemedi!" };
            }
        }

        public ServiceResult<string> basvuruGuncelle(IsBasvurusuDigerBilgiler isBasvurusuDigerBilgiler, long basvuruNo)
        {
            try
            {
                ServiceResult<string> result = wsHelper.wsBasvuruGuncelle(isBasvurusuDigerBilgiler, basvuruNo);
                return result;
            }
            catch (Exception Ex)
            {
                return new ServiceResult<string>() { isSuccessfull = false, ResultCode = 0, ResultData = null, ResultExplanation = "Başvuru Güncellenemedi!" };
            }
        }

    }
}
