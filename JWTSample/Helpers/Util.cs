using JobServiceWcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VakifIlan
{
    public class Util
    {
        public static bool TcKontrol(string tcNo)
        {
            string c = tcNo;

            if (tcNo.Length != 11)
            {
                return false;
            }
            if (!isNumCheck(c))
            {
                return false;
            }

            int n = int.Parse(c.Substring(0, 1));
            if (n == 0)
            {
                return false;
            }
            int m = int.Parse(c.Substring(1, 1));
            int l = int.Parse(c.Substring(2, 1));
            int j = int.Parse(c.Substring(3, 1));
            int i = int.Parse(c.Substring(4, 1));
            int h = int.Parse(c.Substring(5, 1));
            int f = int.Parse(c.Substring(6, 1));
            int d = int.Parse(c.Substring(7, 1));
            int b = int.Parse(c.Substring(8, 1));
            int g = int.Parse(c.Substring(9, 1));
            int e = int.Parse(c.Substring(10, 1));
            if ((10 - ((n + l + i + f + b) * 3 + m + j + h + d) % 10) % 10 != g || (10 - ((m + j + h + d + g) * 3 + n + l + i + f + b) % 10) % 10 != e)
            {
                return false;
            }
            return true;

        }

        public static string CuzdanNoKontrol(string cuzdanNo)
        {
            string s = cuzdanNo;
            int x = 0;
            string result = s;
            bool b = true;
            int i = 0;
            while (b)
            {
                x = int.Parse(s.Substring(i, 1));
                if (x != 0)
                {
                    result = s.Substring(i, s.Length - i);
                    b = false;
                }
                i++;
            }
            return result;
        }

        private static bool isNumCheck(string value)
        {

            if (!string.IsNullOrEmpty(value))
            {
                foreach (char chr in value)
                {
                    if (!Char.IsNumber(chr)) return false;

                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DateBetween(DateTime dateStart, DateTime dateFinish)
        {
            bool isBetween = false;

            if (new DateTime(dateFinish.Year, dateFinish.Month, dateFinish.Day) >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
            {
                if (new DateTime(dateFinish.Year, dateFinish.Month, dateFinish.Day) == new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                {
                    if (DateTime.Now.Hour < 17)
                    {
                        isBetween = true;
                    }
                    else
                    {
                        isBetween = false;
                    }
                }
                else if (new DateTime(dateStart.Year, dateStart.Month, dateStart.Day) <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                {
                    isBetween = true;
                }
            }

            return true;// isBetween;
        }

        private static string ReplaceUnvanText(string sText)
        {
            string sSonuc = sText;

            switch (sText.ToUpper())
            {
                case "BURO_GOREVLISI": sSonuc = "BÜRO GÖREVLİSİ"; break;
                case "MUHASEBECI": sSonuc = "MUHASEBECİ"; break;
                case "SOSYAL_YARDIM_VE_INCELEME_GOREVLISI": sSonuc = "SOSYAL YARDIM VE İNCELEME GÖREVLİSİ"; break;
                case "VAKIF_MUDURU": sSonuc = "VAKIF MÜDÜRÜ"; break;
                case "YARDIMCI_HIZMET_GOREVLISI": sSonuc = "YARDIMCI HİZMET GÖREVLİSİ"; break;
                case "GECICI_PERSONEL": sSonuc = "GEÇİCİ PERSONEL"; break;
                default: sSonuc = sText; break;
            }
            return sSonuc;
        }       

        public static string ReplaceText(string sText)
        {
            string sSonuc = sText;

            switch (sText)
            {
                case "BASVURUDA_BULUNDU": sSonuc = "BAŞVURUDA BULUNDU"; break;
                case "MULAKATA_CAGRILDI": sSonuc = "MÜLAKATA ÇAĞRILDI"; break;
                case "MULAKATA_CAGRILMADI": sSonuc = "MÜLAKATA ÇAĞRILMADI"; break;
                case "KABUL_EDILDI": sSonuc = "KABUL EDİLDİ"; break;
                case "REDDEDILDI": sSonuc = "REDDEDİLDİ"; break;
                case "BURO_GOREVLISI": sSonuc = "BÜRO GÖREVLİSİ"; break;
                case "MUHASEBECI": sSonuc = "MUHASEBECİ"; break;
                case "SOSYAL_YARDIM_VE_INCELEME_GOREVLISI": sSonuc = "SOSYAL YARDIM VE İNCELEME GÖREVLİSİ"; break;
                case "VAKIF_MUDURU": sSonuc = "VAKIF MÜDÜRÜ"; break;
                case "YARDIMCI_HIZMET_GOREVLISI": sSonuc = "YARDIMCI HİZMET GÖREVLİSİ"; break;
                case "BELIRLI_SURELI": sSonuc = "BELİRLİ SÜRELİ"; break;
                case "BELIRSIZ_SURELI": sSonuc = "BELİRSİZ SÜRELİ"; break;
                case "GOREVLENDIRME": sSonuc = "GÖREVLENDİRME"; break;
                case "GECERSIZ": sSonuc = "GEÇERSİZ"; break;
                case "TECILLI": sSonuc = "TECİLLİ"; break;
                case "BOSANMIS": sSonuc = "BOŞANMIŞ"; break;
                case "EVLI": sSonuc = "EVLİ"; break;
                case "ILKOKUL": sSonuc = "İLKOKUL"; break;
                case "ILKOGRETIM": sSonuc = "İLKÖĞRETİM"; break;
                case "LISE": sSonuc = "LİSE"; break;
                case "ON_LISANS": sSonuc = "ÖN LİSANS"; break;
                case "UNIVERSITE": sSonuc = "ÜNİVERSİTE"; break;
                case "YUKSEK_LISANS": sSonuc = "YÜKSEK LİSANS"; break;
                case "GECICI_PERSONEL": sSonuc = "GEÇİCİ PERSONEL"; break;
                default: sSonuc = sText; break;
            }
            return sSonuc;
        }
        public static medeniDurumu getMedeniHalEnumFromStr(string medeniHal)
        {
            medeniDurumu medeniDurumuVal;
            switch (medeniHal)
            {
                case "BEKAR": medeniDurumuVal = medeniDurumu.BEKAR; break;
                case "BILINMEYEN": medeniDurumuVal = medeniDurumu.BILINMEYEN; break;
                case "BOSANMIS": medeniDurumuVal = medeniDurumu.BOSANMIS; break;
                case "DUL": medeniDurumuVal = medeniDurumu.DUL; break;
                case "EVLI": medeniDurumuVal = medeniDurumu.EVLI; break;
                case "EVLILIGIN_FESHI": medeniDurumuVal = medeniDurumu.EVLILIGIN_FESHI; break;
                case "EVLILIGIN_IPTALI": medeniDurumuVal = medeniDurumu.EVLILIGIN_IPTALI; break;
                default: medeniDurumuVal = medeniDurumu.BEKAR; break;
            }
            return medeniDurumuVal;
        }

        public static askerlikDurumu getAskerlikEnumFromStr(string askerlikDurumStr)
        {
            askerlikDurumu askerlikDurumEnum;
            switch (askerlikDurumStr)
            {
                case "GECERSIZ": askerlikDurumEnum = askerlikDurumu.GECERSIZ; break;
                case "MUAF": askerlikDurumEnum = askerlikDurumu.MUAF; break;
                case "TECILLI": askerlikDurumEnum = askerlikDurumu.TECILLI; break;
                case "YAPTI": askerlikDurumEnum = askerlikDurumu.YAPTI; break;
                default: askerlikDurumEnum = askerlikDurumu.MUAF; break;
            }
            return askerlikDurumEnum;

        }

        public static egitimDurumu getEgitimDurumEnumFromStr(string egitimDurumStr)
        {
            egitimDurumu egitimDurumEnum;

            switch (egitimDurumStr.ToUpper())
            {
                case "BILINMIYOR": egitimDurumEnum = egitimDurumu.BILINMIYOR; break;
                case "DOKTORA": egitimDurumEnum = egitimDurumu.DOKTORA; break;
                case "ILKOGRETIM": egitimDurumEnum = egitimDurumu.ILKOGRETIM; break;
                case "ILKOKUL": egitimDurumEnum = egitimDurumu.ILKOKUL; break;
                case "LISE": egitimDurumEnum = egitimDurumu.LISE; break;
                case "LISE_TERK": egitimDurumEnum = egitimDurumu.LISE_TERK; break;
                case "OKUR_YAZAR": egitimDurumEnum = egitimDurumu.OKUR_YAZAR; break;
                case "OKUR_YAZAR_DEGIL": egitimDurumEnum = egitimDurumu.OKUR_YAZAR_DEGIL; break;
                case "OKUR_YAZAR_ILKOGRETIM_TERK": egitimDurumEnum = egitimDurumu.OKUR_YAZAR_ILKOGRETIM_TERK; break;
                case "ON_LISANS": egitimDurumEnum = egitimDurumu.ON_LISANS; break;
                case "ORTAOKUL": egitimDurumEnum = egitimDurumu.ORTAOKUL; break;
                case "UNIVERSITE": egitimDurumEnum = egitimDurumu.UNIVERSITE; break;
                case "UNIVERSITE_TERK": egitimDurumEnum = egitimDurumu.UNIVERSITE_TERK; break;
                case "YUKSEK_LISANS": egitimDurumEnum = egitimDurumu.YUKSEK_LISANS; break;
                default: egitimDurumEnum = egitimDurumu.UNIVERSITE; break;
            }
            return egitimDurumEnum;
        }

        public static cinsiyet getCinsiyetEnumFromStr(string cinsiyet)
        {

            return cinsiyet.ToUpper() == "ERKEK" ? JobServiceWcf.cinsiyet.ERKEK : JobServiceWcf.cinsiyet.KADIN;
        }
    }
}
