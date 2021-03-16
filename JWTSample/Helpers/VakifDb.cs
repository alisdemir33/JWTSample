using JWTSample.Auth;
using JWTSample.AuxClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace VakifIlan
{
    public class VakifDb
    {

        private string connString;

        DataBaseProvider db;
        public VakifDb(string connStr)
        {
            connString = connStr;
            db = new DataBaseProvider(connString);
        }

        public NIslemSonuc<DataTable> GetKullanici(string tcNo, string sifre)
        {
            DataTable dt = null;
            string strSQL = " SET DATEFORMAT DMY SELECT * FROM Personel WHERE TCKimlikNo=@TcKimlikNo AND Parola=@Parola";
            try
            {
               

                db.AddParameter("@TCKimlikNo", DbType.String, tcNo);
                db.AddParameter("@Parola", DbType.String, sifre);

                SqlDataReader dr = db.ExecuteReader(strSQL, CommandType.Text);
                if (dr.HasRows)
                {
                    dt = new DataTable();
                    dt.Load(dr);
                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = true,
                        Veri = dt

                    };
                }
                else
                {
                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = false,
                        Mesaj = "Kullanıcı bulunamadı!"
                    };
                }

            }
            catch (Exception ex)
            {
                return new NIslemSonuc<DataTable>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.ToString(),
                        Sinif = "VakifDb",
                        Metod = "GetKullanici"
                    }
                };
            }


        }

        public NIslemSonuc<DataTable> GetKullanici(string ID)
        {
            DataTable dt = null;
            string strSQL = " SET DATEFORMAT DMY    SELECT * FROM Personel WHERE ID=@ID";
            try
            {
               // DataBaseProvider db = new DataBaseProvider();

                db.AddParameter("@ID", DbType.String, ID);


                SqlDataReader dr = db.ExecuteReader(strSQL, CommandType.Text);
                if (dr.HasRows)
                {
                    dt = new DataTable();
                    dt.Load(dr);
                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = true,
                        Veri = dt

                    };
                }
                else
                {
                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = false,
                        Mesaj = "Kayıt bulunamadı"
                    };
                }

            }
            catch (Exception ex)
            {
                return new NIslemSonuc<DataTable>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.ToString(),
                        Sinif = "VakifDb",
                        Metod = "GetKullanici"
                    }
                };
            }
        }

        public NIslemSonuc<bool> SetPersonelBilgilendirme(string sIP, string sPersonelID)
        {
            string strSQL = "UPDATE Personel SET AgreementAccept=1,GuncellemeTarihi=GETDATE(),GuncellemeIP=@KayitIP WHERE ID=@ID";

           // DataBaseProvider db = new DataBaseProvider();

            try
            {
                db.AddParameter("@KayitIP", DbType.String, sIP);
                db.AddParameter("@ID", DbType.Int64, sPersonelID);

                int i = db.ExecuteNonQuery(strSQL, CommandType.Text);

                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true,
                        Veri = true
                    };
                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }
            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        Metod = "SetPersonelBilgilendirme",
                        Sinif = "VakifDb",
                        HataMesaj = ex.Message
                    }
                };
            }


        }

        public NIslemSonuc<bool> SetRefreshToken(Token token, string sApplicantID)
        {
            string strSQL = "SET Dateformat DMY; UPDATE Personel SET refreshToken=@RefreshToken, RefreshTokenEndDate=@RefreshTokenEndDate WHERE ID=@ID";

           // DataBaseProvider db = new DataBaseProvider();

            try
            {
                db.AddParameter("@RefreshToken", DbType.String, token.RefreshToken);
                db.AddParameter("@RefreshTokenEndDate", DbType.String, token.RefreshTokenExpirationDate);
                db.AddParameter("@ID", DbType.Int64, sApplicantID);

                int i = db.ExecuteNonQuery(strSQL, CommandType.Text);

                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true,
                        Veri = true
                    };
                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }
            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        Metod = "SetRefreshToken",
                        Sinif = "VakifDb",
                        HataMesaj = ex.Message
                    }
                };
            }


        }


        public NIslemSonuc<DataTable> CheckRefreshToken(string refreshToken)
        {
            DataTable dt = null;
            string strSQL = " SET DATEFORMAT DMY    SELECT * FROM Personel WHERE RefreshToken=@refreshToken";
            try
            {
               // DataBaseProvider db = new DataBaseProvider();

                db.AddParameter("@RefreshToken", DbType.String, refreshToken);

                SqlDataReader dr = db.ExecuteReader(strSQL, CommandType.Text);
                if (dr.HasRows)
                {
                    dt = new DataTable();
                    dt.Load(dr);
                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = true,
                        Veri = dt

                    };
                }
                else
                {
                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = false,
                        Mesaj = "Kayıt bulunamadı"
                    };
                }

            }
            catch (Exception ex)
            {
                return new NIslemSonuc<DataTable>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.ToString(),
                        Sinif = "VakifDb",
                        Metod = "GetKullanici"
                    }
                };
            }
        }


        public NIslemSonuc<bool> AddPersonelSqlLog(string sPersonelID, string sSqlError, string sIP)
        {
            string strSQL = @"INSERT INTO PersonelSqlLog (PersonelID,SqlError,KayitTarihi,IP) 
                            VALUES(@PersonelID,@SqlError,GetDate(),@IP)";

           // DataBaseProvider db = new DataBaseProvider();

            db.AddParameter("@PersonelID", DbType.Int64, sPersonelID);
            db.AddParameter("@SqlError", DbType.String, sSqlError);
            db.AddParameter("@IP", DbType.String, sIP);

            try
            {
                int i = db.ExecuteNonQuery(strSQL, CommandType.Text);
                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true
                    };

                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }
            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        Metod = "AddPersonelSqlLog",
                        Sinif = "VakifDb",
                        HataMesaj = ex.Message
                    }
                };
            }

        }

        public NIslemSonuc<bool> AddPersonelInfoLog(PersonelInfo personelInfo)
        {


            string strSQL = @"INSERT INTO BasvuranInfoLog (TCKimlikNo,LogTime,MernisAd,MernisSoyad,MernisSeriNo,BasvuranAd,BasvuranSoyad,BasvuranSeriNo,LogType) 
                            VALUES(@TCKimlikNo,getdate(), @MernisAd, @MernisSoyad, @MernisSeriNo, @BasvuranAd, @BasvuranSoyad, @BasvuranSeriNo, @LogType)";

          //  DataBaseProvider db = new DataBaseProvider();


            db.AddParameter("@TCKimlikNo", DbType.String, personelInfo.TCKimlikNo);

            db.AddParameter("@MernisAd", DbType.String, personelInfo.MernisAd);
            db.AddParameter("@MernisSoyad", DbType.String, personelInfo.MernisSoyad);
            db.AddParameter("@MernisSeriNo", DbType.String, personelInfo.MernisSeriNo);

            db.AddParameter("@BasvuranAd", DbType.String, personelInfo.BasvuranAd);
            db.AddParameter("@BasvuranSoyad", DbType.String, personelInfo.BasvuranSoyad);
            db.AddParameter("@BasvuranSeriNo", DbType.String, personelInfo.BasvuranSeriNo);

            db.AddParameter("@LogType", DbType.String, personelInfo.LogType);

            try
            {
                int i = db.ExecuteNonQuery(strSQL, CommandType.Text);
                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true
                    };

                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }
            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        Metod = "AddPersonelInfoLog",
                        Sinif = "VakifDb",
                        HataMesaj = ex.Message
                    }
                };
            }

        }


        public NIslemSonuc<DataTable> GetIller()
        {
            string strSQL = "SELECT DISTINCT Il_Kodu,Il_Adi FROM MernisIl_Ilce_Kodlari ORDER BY Il_Adi";

          //  DataBaseProvider db = new DataBaseProvider();

            try
            {
                SqlDataReader dr = db.ExecuteReader(strSQL, CommandType.Text);
                if (dr.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = true,
                        Veri = dt
                    };
                }
                else
                {
                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = false
                    };

                }

            }
            catch (Exception ex)
            {

                return new NIslemSonuc<DataTable>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.Message,
                        Sinif = "VakifDb",
                        Metod = "GetIller"
                    }
                };
            }
        }

        public NIslemSonuc<DataTable> GetIlceByIL(string il)
        {
            string strSQL = "SELECT DISTINCT Ilce_Kodu,Ilce_Adi FROM MernisIl_Ilce_Kodlari WHERE Il_Kodu=@Il_Kodu ORDER BY Ilce_Adi";

          //  DataBaseProvider db = new DataBaseProvider();

            db.AddParameter("@Il_Kodu", DbType.Int32, il);

            try
            {
                SqlDataReader dr = db.ExecuteReader(strSQL, CommandType.Text);
                if (dr.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = true,
                        Veri = dt
                    };
                }
                else
                {
                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = false
                    };

                }

            }
            catch (Exception ex)
            {

                return new NIslemSonuc<DataTable>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.Message,
                        Sinif = "VakifDb",
                        Metod = "GetIlceByIL"
                    }
                };
            }
        }

        public NIslemSonuc<bool> AddPersonelError(string sPersonelID, string sIl, string sIlce, string sUnvan, string sIlanNo, string sBasvuruNo, string sHata, string sIp)
        {
            string strSQL = @"INSERT INTO  PersonelError (PersonelID,Il,Ilce,Unvan,IlanNo,BasvuruNo,Hata,KayitTarihi,IP)
            VALUES(@PersonelID,@Il,@Ilce,@Unvan,@IlanNo,@BasvuruNo,@Hata,GETDATE(),@IP)";

         //   DataBaseProvider db = new DataBaseProvider();

            db.AddParameter("@PersonelID", DbType.Int64, sPersonelID);
            db.AddParameter("@Il", DbType.Int32, sIl);
            db.AddParameter("@Ilce", DbType.Int32, sIlce);
            db.AddParameter("@Unvan", DbType.String, sUnvan);
            db.AddParameter("@IlanNo", DbType.Int64, sIlanNo);
            db.AddParameter("@BasvuruNo", DbType.Int64, sBasvuruNo == "" ? "0" : sBasvuruNo);
            db.AddParameter("@Hata", DbType.String, sHata);
            db.AddParameter("@IP", DbType.String, sIp);

            try
            {
                int i = db.ExecuteNonQuery(strSQL, CommandType.Text);
                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true
                    };
                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }
            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.Message,
                        Sinif = "VakifDb",
                        Metod = "AddPersonelError"
                    }

                };
            }
        }

        public NIslemSonuc<bool> SetPersonData(string sPersonelID, string sAskerlikDurum, string sEgitimDurum, string sMezuniyetTarihi, string sUniversiteBolum, string sKpssYil, string sKpssPuan, string sEvTelefon, string sIsTelefon, string sCepTelefon, string sEposta, string sIp, string sTecilTarihi, string[] sAddress)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"SET DATEFORMAT DMY UPDATE  Personel   SET AskerlikDurumu =@AskerlikDurumu ,EgitimDurumu =@EgitimDurumu ,MezuniyetTarihi =@MezuniyetTarihi ,UniversiteBolum =@UniversiteBolum  ,KPSSYil =@KPSSYil,KPSSPuan =@KPSSPuan ,EvTelefonu =@EvTelefonu ,IsTelefonu =@IsTelefonu  ,CepTelefonu =@CepTelefonu, EPosta =@EPosta, GuncellemeTarihi = GETDATE(),GuncellemeIP =@GuncellemeIP");

           // DataBaseProvider db = new DataBaseProvider();

            db.AddParameter("@AskerlikDurumu", DbType.String, sAskerlikDurum);
            db.AddParameter("@EgitimDurumu", DbType.String, sEgitimDurum);
            db.AddParameter("@MezuniyetTarihi", DbType.DateTime, sMezuniyetTarihi);
            db.AddParameter("@UniversiteBolum", DbType.String, sUniversiteBolum);
            db.AddParameter("@KPSSYil", DbType.Int32, sKpssYil);
            db.AddParameter("@KPSSPuan", DbType.Double, Convert.ToDouble(sKpssPuan));
            db.AddParameter("@EvTelefonu", DbType.String, sEvTelefon);
            db.AddParameter("@IsTelefonu", DbType.String, sIsTelefon);
            db.AddParameter("@CepTelefonu", DbType.String, sCepTelefon);
            db.AddParameter("@EPosta", DbType.String, sEposta);
            db.AddParameter("@GuncellemeIP", DbType.String, sIp);

            if (sAddress != null && sAddress.Length > 3)
            {
                sb.Append("  ,AdresIlKodu =@AdresIlKodu ,AdresIlceKodu =@AdresIlceKodu ,AdresIl =@AdresIl,AdresIlce=@AdresIlce ");
                db.AddParameter("@AdresIlKodu", DbType.Int32, sAddress[3]);
                db.AddParameter("@AdresIlceKodu", DbType.Int32, sAddress[1]);
                db.AddParameter("@AdresIl", DbType.String, sAddress[2]);
                db.AddParameter("@AdresIlce", DbType.String, sAddress[0]);
            }


            if (sTecilTarihi != "")
            {
                DateTime tecil = DateTime.Now;
                if (DateTime.TryParse(sTecilTarihi, out tecil))
                {
                    sb.Append(" ,TecilTarihi=@TecilTarihi");
                    db.AddParameter("@TecilTarihi", DbType.DateTime, Convert.ToDateTime(sTecilTarihi).ToString("dd/MM/yyyy"));
                }

            }

            sb.Append(" WHERE ID=@ID");
            db.AddParameter("@ID", DbType.Int64, sPersonelID);

            try
            {
                int i = db.ExecuteNonQuery(sb.ToString(), CommandType.Text);
                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true
                    };
                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }
            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.Message,
                        Sinif = "VakifDb",
                        Metod = "SetPersonelData"
                    }
                };
            }



        }

        public NIslemSonuc<bool> SetPersonelBasvuru(string sPersonelID, string sIlanNo, string sBasvuruNo)
        {
            string strSQL = "UPDATE  PersonelBasvuru SET BasvuruNo=@BasvuruNo WHERE   PersonelID=@PersonelID  AND  IlanNo=@IlanNo";

          // DataBaseProvider db = new DataBaseProvider();

            try
            {
                db.AddParameter("@BasvuruNo", DbType.Int64, sBasvuruNo);
                db.AddParameter("@PersonelID", DbType.Int64, sPersonelID);
                db.AddParameter("@IlanNo", DbType.Int64, sIlanNo);


                int i = db.ExecuteNonQuery(strSQL, CommandType.Text);
                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true
                    };
                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }

            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.Message,
                        Metod = "SetPersonelBasvuru",
                        Sinif = "VakifIlan"
                    }
                };
            }


        }

        public NIslemSonuc<bool> AddPersonelBasvuru(string sPersonelID, string sIl, string sIlce, string sUnvan, string sIlanNo, string sIp)
        {
            string strSQL = @"INSERT INTO  PersonelBasvuru (PersonelID,Il,Ilce,Unvan,IlanNo,KayitTarihi,KayitIP,State,ConditionsAgreement) 
                    VALUES (@PersonelID,@Il,@Ilce,@Unvan,@IlanNo,GetDate(),@KayitIP,1,1) ";

           // DataBaseProvider db = new DataBaseProvider();

            db.AddParameter("@PersonelID", DbType.Int64, sPersonelID);
            db.AddParameter("@Il", DbType.Int32, sIl);
            db.AddParameter("@Ilce", DbType.Int32, sIlce);
            db.AddParameter("@Unvan", DbType.String, sUnvan);
            db.AddParameter("@IlanNo", DbType.Int64, sIlanNo);
            db.AddParameter("@KayitIP", DbType.String, sIp);
            try
            {
                int i = db.ExecuteNonQuery(strSQL, CommandType.Text);
                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true
                    };
                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }
            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.Message
                    }
                };
            }
        }


        public NIslemSonuc<bool> DeleteBasvuru(string sBasvuruNo, string sIp, string sPersonelID, string sIl, string sIlce, string sUnvan, string sIlanNo)
        {
            string strSQL = @" UPDATE      PersonelBasvuru SET BasvuruNo =@BasvuruNo,KayitTarihi = GETDATE(),KayitIP =@KayitIP,State =0 WHERE PersonelID =@PersonelID AND Il =@Il AND Ilce =@Ilce AND Unvan =@Unvan AND    IlanNo =@IlanNo";

          //  DataBaseProvider db = new DataBaseProvider();

            db.AddParameter("@BasvuruNo", DbType.Int64, sBasvuruNo);
            db.AddParameter("@KayitIP", DbType.String, sIp);
            db.AddParameter("@PersonelID", DbType.Int64, sPersonelID);
            db.AddParameter("@Il", DbType.Int32, sIl);
            db.AddParameter("@Ilce", DbType.Int32, sIlce);
            db.AddParameter("@Unvan", DbType.String, sUnvan);
            db.AddParameter("@IlanNo", DbType.Int64, sIlanNo);

            try
            {
                int i = db.ExecuteNonQuery(strSQL, CommandType.Text);
                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true
                    };
                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }
            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.Message,
                        Metod = "DeleteBasvuru"
                    }
                };
            }

        }

        public NIslemSonuc<bool> SetPersonelPassWord(string sParola, string sPersonelID, string sIP)
        {
            string strSQL = "UPDATE  Personel SET  Parola=@Parola,GuncellemeTarihi = GETDATE(),GuncellemeIP =@GuncellemeIP WHERE   ID=@ID";

           // DataBaseProvider db = new DataBaseProvider();

            db.AddParameter("@Parola", DbType.String, sParola);
            db.AddParameter("@GuncellemeIP", DbType.String, sIP);
            db.AddParameter("@ID", DbType.Int64, sPersonelID);

            try
            {
                int i = db.ExecuteNonQuery(strSQL, CommandType.Text);
                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true
                    };
                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }
            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.Message,
                        Metod = "SetPersonelPassWord"
                    }

                };
            }
        }

        public NIslemSonuc<DataTable> GetPersonelPassWord(string tcNo)
        {
            string strSQL = "SELECT Adi,Soyadi,Parola,EPosta  FROM Personel WHERE TCKimlikNo=@TCKimlikNo";

        //    DataBaseProvider db = new DataBaseProvider();

            db.AddParameter("@TCKimlikNo", DbType.String, tcNo);
            try
            {
                SqlDataReader dr = db.ExecuteReader(strSQL, CommandType.Text);
                if (dr.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(dr);

                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = true,
                        Veri = dt
                    };
                }
                else
                {
                    return new NIslemSonuc<DataTable>
                    {
                        BasariliMi = false
                    };
                }
            }
            catch (Exception ex)
            {

                return new NIslemSonuc<DataTable>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.Message
                    }
                };
            }
        }

        public NIslemSonuc<bool> AddPersonel(string sTcNo, string sAdi, string sSoyadi, string sDogumTarihi, string sDogumYeri, string sCinsiyet, string sMedeniHal, string sEposta, string sParola, string sIP)
        {
            string strSQL = @" SET DATEFORMAT DMY 
                                    SELECT ID FROM Personel WHERE TCKimlikNo=@TCKimlikNo
                                        IF @@ROWCOUNT=0
	                                    INSERT  INTO Personel (TCKimlikNo,Adi,Soyadi,DogumTarihi,DogumYeri,Cinsiyeti,MedeniHali,EPosta,Parola,KayitTarihi,KayitIP) 
                                        VALUES (@TCKimlikNo,@Adi,@Soyadi,@DogumTarihi,@DogumYeri,@Cinsiyeti,@MedeniHali,@EPosta,@Parola,GetDate(),@KayitIP)
                                    ELSE
	                                    UPDATE	Personel SET Adi=@Adi,Soyadi=@Soyadi,EPosta=@EPosta, Parola=@Parola, GuncellemeTarihi=GetDate(), GuncellemeIP=@KayitIP  WHERE	TCKimlikNo=@TCKimlikNo";

         //   DataBaseProvider db = new DataBaseProvider();

            try
            {
                db.AddParameter("@TCKimlikNo", DbType.String, sTcNo);
                db.AddParameter("@Adi", DbType.String, sAdi);
                db.AddParameter("@Soyadi", DbType.String, sSoyadi);
                db.AddParameter("@DogumTarihi", DbType.DateTime, sDogumTarihi);
                db.AddParameter("@DogumYeri", DbType.String, sDogumYeri);
                db.AddParameter("@Cinsiyeti", DbType.String, sCinsiyet);
                db.AddParameter("@MedeniHali", DbType.String, sMedeniHal);
                db.AddParameter("@EPosta", DbType.String, sEposta);
                db.AddParameter("@Parola", DbType.String, sParola);
                db.AddParameter("@KayitIP", DbType.String, sIP);

                int i = db.ExecuteNonQuery(strSQL, CommandType.Text);
                if (i > 0)
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = true
                    };
                }
                else
                {
                    return new NIslemSonuc<bool>
                    {
                        BasariliMi = false
                    };
                }

            }
            catch (Exception ex)
            {

                return new NIslemSonuc<bool>
                {
                    BasariliMi = false,
                    HataBilgi = new NHata
                    {
                        HataMesaj = ex.Message,
                        Metod = "AddPersonel"
                    }
                };
            }


        }
    }
}