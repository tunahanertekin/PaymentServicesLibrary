using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.Web;
using System.Collections.Specialized;

using PaymentServicesLibrary.Modeller;

namespace PaymentServicesLibrary.Finansbank
{
    public class FinansbankSanalPos
    {
        public String PosFinansbank(Pos Cpos, KKart Ckkart, Kullanici Ckullanici, PosIslem CPosIslem)
        {
            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            String strMbrId = "5";                                                                //Kurum Kodu
            String strMerchantID = PosBilgi[0];//posbilgi                                                      //Language_MerchantID
            String strMerchantPass = PosBilgi[1];//posbilgi                                                  //Language_MerchantPass
            String strUserCode = PosBilgi[2];                                                          //Kullanici Kodu
            String strSecureType = "3DPay";                                                             //Language_SecureType
            String strTxnType = "Auth";                                                                 //Islem Tipi
            String strInstallmentCount = CPosIslem.Taksit; //taksit                                                          //Taksit Sayisihttp://localhost:64158/App_Data/
            String strCurrency = "949";                                                          //Para Birimi
            String strOkUrl = GenelMetotlar.getDomain() + "/" + Cpos.PosDonus; //dönüş                                                               //Language_OkUrl
            String strFailUrl = GenelMetotlar.getDomain() + "/" + Cpos.PosDonus; //dönüş                                                           //Language_FailUrl
            String strOrderId = "";                                                            //Siparis Numarasi
            String strOrgOrderId = "";                                                      //Orijinal Islem Siparis Numarasi
            String strAmount = CPosIslem.Odeme ; //miktar                                                               //Tutar
            String strPurchAmount = (double.Parse(strAmount)/100).ToString().Replace(".", ",");
            
            String strLang = "TR";                                                                  //Language_Lang

            String strrnd = DateTime.Now.Ticks.ToString();
            String str = strMbrId + strOrderId + strPurchAmount + strOkUrl + strFailUrl + strTxnType + strInstallmentCount + strrnd + strMerchantPass;
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] hashingbytes = sha.ComputeHash(bytes);
            String strhash = Convert.ToBase64String(hashingbytes);

            String cardholder = Ckullanici.KullaniciAdi;
            String pan = Ckkart.KartNo;
            String cvv = Ckkart.cvc2;
            String expiry = Ckkart.SonKulAy + Ckkart.SonKulYil;


            String formContent =
            "<form id=\"parameters\" method=\"post\" action=\"https://vpos.qnbfinansbank.com/Gateway/Default.aspx\">" +
                "<input type=\"hidden\" name=\"CardHolderName\" value=\"" + cardholder + "\" />" +
                "<input type=\"hidden\" name=\"Pan\" value=\"" + pan + "\" />" +
                "<input type=\"hidden\" name=\"Cvv2\" value=\"" + cvv + "\" />" +
                "<input type=\"hidden\" name=\"Expiry\" value=\"" + expiry + "\" />" +

                "<input type=\"hidden\" name=\"MbrId\" value=\"" + strMbrId + "\" />" +
                "<input type=\"hidden\" name=\"MerchantID\" value=\"" + strMerchantID + "\" />" +
                "<input type=\"hidden\" name=\"UserCode\" value=\"" + strUserCode + "\" />" +
                "<input type=\"hidden\" name=\"SecureType\" value=\"" + strSecureType + "\" />" +
                "<input type=\"hidden\" name=\"TxnType\" value=\"" + strTxnType + "\" />" +
                "<input type=\"hidden\" name=\"InstallmentCount\" value=\"" + strInstallmentCount + "\" />" +
                "<input type=\"hidden\" name=\"Currency\" value=\"" + strCurrency + "\" />" +
                "<input type=\"hidden\" name=\"OkUrl\" value=\"" + strOkUrl + "\" />" +
                "<input type=\"hidden\" name=\"FailUrl\" value=\"" + strFailUrl + "\" />" +
                "<input type=\"hidden\" name=\"OrderId\" value=\"" + strOrderId + "\" />" +
                "<input type=\"hidden\" name=\"OrgOrderId\" value=\"" + strOrgOrderId + "\" />" +
                "<input type=\"hidden\" name=\"PurchAmount\" value=\"" + strPurchAmount + "\" />" +
                "<input type=\"hidden\" name=\"Lang\" value=\"" + strLang + "\" />" +
                "<input type=\"hidden\" name=\"Rnd\" value=\"" + strrnd + "\" />" +
                "<input type=\"hidden\" name=\"Hash\" value=\"" + strhash + "\" />" +
            "</form>" +
            "<script type=\"text/javascript\">document.getElementById(\"parameters\").submit();</script>";


            return formContent;
        }


        public PosYanit PosFinansbankDonus(Pos Cpos)
        {
            PosYanit Yanit = new PosYanit();
            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            NameValueCollection nvc = HttpContext.Current.Request.Form;

            String MerchantID = PosBilgi[0];
            String MerchantPass = PosBilgi[1];
            String OrderId = "";
            String AuthCode = "";
            String ProcReturnCode = "";
            String Status3D = "";
            String ResponseRnd = "";
            String UserCode = PosBilgi[2];

            if (nvc["OrderId"] != null)
            {
                OrderId = nvc["OrderId"];
            }
            if (nvc["AuthCode"] != null)
            {
                AuthCode = nvc["AuthCode"];
            }
            if (nvc["ProcReturnCode"] != null)
            {
                ProcReturnCode = nvc["ProcReturnCode"];
            }
            if (nvc["3DStatus"] != null)
            {
                Status3D = nvc["3DStatus"];
            }
            if (nvc["ResponseRnd"] != null)
            {
                ResponseRnd = nvc["ResponseRnd"];
            }



            if (nvc["3DStatus"].Equals("1"))
            {
                if (ProcReturnCode.Equals("00"))
                {
                    String str = MerchantID + MerchantPass + OrderId + AuthCode + ProcReturnCode + Status3D + ResponseRnd + UserCode;

                    System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
                    byte[] hashingbytes = sha.ComputeHash(bytes);
                    String ResponseHash = Convert.ToBase64String(hashingbytes);

                    if (ResponseHash.Equals(nvc["ResponseHash"]))
                    {
                        Yanit.Onay = 1;
                        Yanit.IslemNo = AuthCode;
                        Yanit.Hata = "Hata yok";
                        Yanit.detay = "";
                        Yanit.Odeme = Double.Parse(nvc["PurchAmount"].Replace(",", "."));
                    }
                    else
                    {
                        Yanit.Onay = 0;
                        Yanit.IslemNo = "0";
                        Yanit.Hata = "Hash değeri uyuşmuyor. --> " + nvc["ErrMsg"];
                    }
                }
                else
                {
                    Yanit.Onay = 0;
                    Yanit.IslemNo = "0";
                    Yanit.Hata = "3D İşlemi başarılı, ödeme başarısız." + nvc["ErrMsg"];
                }
            }
            else
            {
                Yanit.Onay = 0;
                Yanit.IslemNo = "0";
                Yanit.Hata = "3D ve ödeme işlemi başarısız. -->" + nvc["ErrMsg"];
            }

            //Yanit.PosID = int.Parse(HttpContext.Current.Session["POSID"].ToString());//Session'dan al.
            //HttpContext.Current.Session["POSID"] = "";
            //HttpContext.Current.Session["EkBilgiler"] = "";
            //Session'u sıfırla.


            return Yanit;
        }

    }
}
