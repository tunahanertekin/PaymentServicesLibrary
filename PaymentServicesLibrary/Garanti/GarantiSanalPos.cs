using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Specialized;
using System.Web;

using RestSharp;

using PaymentServicesLibrary.Modeller;

namespace PaymentServicesLibrary.Garanti
{
    public class GarantiSanalPos
    {

        //(Cpos As Pos, Ckkart As KKart, Ckullanici As Kullanici, CPosislem As PosIslem)
        public String PosGaranti3D(Pos Cpos, KKart Ckkart, Kullanici Ckullanici, PosIslem CPosIslem)
        {
            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            string strMode = "PROD";
            string strApiVersion = "v0.01";
            string strTerminalProvUserID = "PROVAUT";
            string strType = "sales";
            string strAmount = CPosIslem.Odeme; //İşlem Tutarı 1.00 TL için 100 gönderilmeli
            string strCurrencyCode = "949";
            string strInstallmentCount = CPosIslem.Taksit; //Taksit Sayısı. Boş gönderilirse taksit yapılmaz
            string strTerminalUserID = "010101";
            string strOrderID = Guid.NewGuid().ToString().Replace("-", "_");
            string strCustomeripaddress = GenelMetotlar.getIP(); //Kullanıcının IP adresini alır
            string strcustomeremailaddress = Ckullanici.KullaniciEmail;
            string strTerminalID = PosBilgi[0]; //TerminalID yazılmalı.
            string _strTerminalID = "0" + strTerminalID;
            string strTerminalMerchantID = PosBilgi[1];
            string strStoreKey = PosBilgi[2]; //3D Secure şifresi
            string strProvisionPassword = PosBilgi[3]; //TerminalProvUserID şifresi
            string strSuccessURL = GenelMetotlar.getDomain() + "/" + Cpos.PosDonus;//"https://localhost:44349/Return.aspx";
            string strErrorURL = GenelMetotlar.getDomain() + "/" + Cpos.PosDonus;
            string SecurityData = GenelMetotlar.GetSHA1(strProvisionPassword + _strTerminalID).ToUpper();
            string HashData = GenelMetotlar.GetSHA1(strTerminalID + strOrderID + strAmount + strSuccessURL + strErrorURL + strType + strInstallmentCount + strStoreKey + SecurityData).ToUpper();

            String formContent = "<form id=\"parameters\" method=\"POST\" action='https://sanalposprov.garanti.com.tr/servlet/gt3dengine'>" +
            "<input type=\"hidden\" name=\"secure3dsecuritylevel\" value=\"3D_PAY\" />" +
            "<input type=\"hidden\" name=\"cardnumber\" value=\"" + Ckkart.KartNo + "\" />" +
            "<input type=\"hidden\" name=\"cardexpiredatemonth\" value=\"" + Ckkart.SonKulAy + "\" />" +
            "<input type=\"hidden\" name=\"cardexpiredateyear\" value=\"" + Ckkart.SonKulYil + "\" />" +
            "<input type=\"hidden\" name=\"cardcvv2\" value=\"" + Ckkart.cvc2 + "\" />" +

            "<input type=\"hidden\" name=\"mode\" value=\"" + strMode + "\" />" +
            "<input type=\"hidden\" name=\"apiversion\" value=\"" + strApiVersion + "\" />" +
            "<input type=\"hidden\" name=\"terminalprovuserid\" value=\"" + strTerminalProvUserID + "\" />" +
            "<input type=\"hidden\" name=\"terminaluserid\" value=\"" + strTerminalUserID + "\" />" +
            "<input type=\"hidden\" name=\"terminalmerchantid\" value=\"" + strTerminalMerchantID + "\" />" +

            "<input type=\"hidden\" name=\"txntype\" value=\"" + strType + "\" />" +
            "<input type=\"hidden\" name=\"txnamount\" value=\"" + strAmount + "\" />" +
            "<input type=\"hidden\" name=\"txncurrencycode\" value=\"" + strCurrencyCode + "\" />" +
            "<input type=\"hidden\" name=\"txninstallmentcount\" value=\"" + strInstallmentCount + "\" />" +
            "<input type=\"hidden\" name=\"customeremailaddress\" value=\"" + strcustomeremailaddress + "\" />" +

            "<input type=\"hidden\" name=\"customeripaddress\" value=\"" + strCustomeripaddress + "\" />" +
            "<input type=\"hidden\" name=\"orderid\" value=\"" + strOrderID + "\" />" +
            "<input type=\"hidden\" name=\"terminalid\" value=\"" + strTerminalID + "\" />" +
            "<input type=\"hidden\" name=\"successurl\" value=\"" + strSuccessURL + "\" />" +
            "<input type=\"hidden\" name=\"errorurl\" value=\"" + strErrorURL + "\" />" +

            "<input type=\"hidden\" name=\"secure3dhash\" value=\"" + HashData + "\" />" +

            "<input type=\"hidden\" name=\"secure3dhash\" value=\"" + HashData + "\" />" +
            "<input type=\"hidden\" name=\"secure3dhash\" value=\"" + HashData + "\" />" +
            "<input type=\"hidden\" name=\"secure3dhash\" value=\"" + HashData + "\" />" +
            "<input type=\"hidden\" name=\"secure3dhash\" value=\"" + HashData + "\" />" +

            "</form>" +
            "<script type=\"text/javascript\">document.getElementById(\"parameters\").submit();</script>";

            return formContent;
        }

        public PosYanit PosGaranti3DDonus(Pos Cpos)
        {
            PosYanit Yanit = new PosYanit();

            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            NameValueCollection nvc = HttpContext.Current.Request.Form;

            String strMode = nvc["mode"];
            String strApiVersion = nvc["apiversion"];
            String strTerminalProvUserID = nvc["terminalprovuserid"];
            String strType = nvc["txntype"];
            String strAmount = nvc["txnamount"];

            String strCurrencyCode = nvc["txncurrencycode"];
            String strInstallmentCount = nvc["txninstallmentcount"];
            String strTerminalUserID = nvc["terminaluserid"];
            String strOrderID = nvc["oid"];
            String strCustomeripaddress = nvc["customeripaddress"];

            String strcustomeremailaddress = nvc["customeremailaddress"];
            String strTerminalID = nvc["clientid"];
            String _strTerminalID = "0" + strTerminalID;
            String strTerminalMerchantID = nvc["terminalmerchantid"];
            String strStoreKey = PosBilgi[2];

            String strProvisionPassword = nvc["mode"];
            String strSuccessURL = nvc["mode"];
            String strErrorURL = nvc["mode"];
            String strCardholderPresentCode = "13";
            String strMotoInd = "N";

            String strNumber = "";
            String strExpireDate = "";
            String strCVV2 = "";
            String strAuthenticationCode = HttpUtility.UrlEncode(nvc["cavv"]);
            String strSecurityLevel = HttpUtility.UrlEncode(nvc["eci"]);

            String strTxnID = HttpUtility.UrlEncode(nvc["xid"]);
            String strMD = HttpUtility.UrlEncode(nvc["md"]);
            String strMDStatus = nvc["mdstatus"];
            String strMDStatusText = nvc["mdErrorMsg"];
            String strHostAddress = "https://sanalposprov.garanti.com.tr/VPServlet";

            String SecurityData = GenelMetotlar.GetSHA1(strProvisionPassword + _strTerminalID).ToUpper();
            String HashData = GenelMetotlar.GetSHA1(strTerminalID + strOrderID + strAmount + strSuccessURL + strErrorURL + strType + strInstallmentCount + strStoreKey + SecurityData).ToUpper();

            if (strMDStatus.Equals("1"))
            {
                strMDStatusText = "Tam Doğrulama";
            }
            else if (strMDStatus.Equals("2"))
            {
                strMDStatusText = "Kart Sahibi veya bankası sisteme kayıtlı değil";
            }
            else if (strMDStatus.Equals("3"))
            {
                strMDStatusText = "Kartın bankası sisteme kayıtlı değil";
            }
            else if (strMDStatus.Equals("4"))
            {
                strMDStatusText = "Doğrulama denemesi, kart sahibi sisteme daha sonra kayıt olmayı seçmiş";
            }
            else if (strMDStatus.Equals("5"))
            {
                strMDStatusText = "Doğrulama yapılamıyor";
            }
            else if (strMDStatus.Equals("6"))
            {
                strMDStatusText = "3-D Secure Hatası";
            }
            else if (strMDStatus.Equals("7"))
            {
                strMDStatusText = "Sistem Hatası";
            }
            else if (strMDStatus.Equals("8"))
            {
                strMDStatusText = "Bilinmeyen Kart No";
            }
            else if (strMDStatus.Equals("0"))
            {
                strMDStatusText = "Doğrulama Başarısız, 3-D Secure imzası geçersiz.";
            }

            String strHashData = nvc["secure3dhash"];
            String validateHashData = GenelMetotlar.GetSHA1(strTerminalID + strOrderID + strAmount + strSuccessURL + strErrorURL + strType + strInstallmentCount + strStoreKey + SecurityData).ToUpper();

            if (strHashData.Equals(validateHashData))
            {
                if (strMDStatus.Equals("1") || strMDStatus.Equals("2") || strMDStatus.Equals("3") || strMDStatus.Equals("4"))
                {
                    String strXML =

                        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                        "<GVPSRequest>" +

                            "<Mode>" + strMode + "</Mode>" +

                            "<Version>" + strApiVersion + "</Version>" +

                            "<ChannelCode></ChannelCode>" +

                            "<Terminal>" +
                                "<ProvUserID>" + strTerminalProvUserID + "</ProvUserID>" +
                                "<HashData>" + HashData + "</HashData>" +
                                "<UserID>" + strTerminalUserID + "</UserID>" +
                                "<ID>" + strTerminalID + "</ID>" +
                                "<MerchantID>" + strTerminalMerchantID + "</MerchantID>" +
                            "</Terminal>" +

                            "<Customer>" +
                                "<IPAddress>" + strCustomeripaddress + "</IPAddress>" +
                                "<EmailAddress>" + strcustomeremailaddress + "</EmailAddress>" +
                            "</Customer>" +

                            "<Card>" +
                                "<Number></Number>" +
                                "<ExpireDate></ExpireDate>" +
                                "<CVV2></CVV2>" +
                            "</Card>" +

                            "<Order>" +
                                "<OrderID>" + strOrderID + "</OrderID>" +
                                "<GroupID></GroupID>" +
                                "<AddressList>" +
                                    "<Address>" +
                                        "<Type>S</Type>" +
                                        "<Name></Name>" +
                                        "<LastName></LastName>" +
                                        "<Company></Company>" +
                                        "<Text></Text>" +
                                        "<District></District>" +
                                        "<City></City>" +
                                        "<PostalCode></PostalCode>" +
                                        "<Country></Country>" +
                                        "<PhoneNumber></PhoneNumber>" +
                                    "</Address>" +
                                "</AddressList>" +
                            "</Order>" +

                            "<Transaction>" +
                                "<Type>" + strType + "</Type>" +
                                "<InstallmentCnt>" + strInstallmentCount + "</InstallmentCnt>" +
                                "<Amount>" + strAmount + "</Amount>" +
                                "<CurrencyCode>" + strCurrencyCode + "</CurrencyCode>" +
                                "<CardholderPresentCode>" + strCardholderPresentCode + "</CardholderPresentCode>" +
                                "<MotoInd>" + strMotoInd + "</MotoInd>" +
                                "<Secure3D>" +
                                    "<AuthenticationCode>" + strAuthenticationCode + "</AuthenticationCode>" +
                                    "<SecurityLevel>" + strSecurityLevel + "</SecurityLevel>" +
                                    "<TxnID>" + strTxnID + "</TxnID>" +
                                    "<Md>" + strMD + "</Md>" +
                                "</Secure3D>" +
                            "</Transaction>" +

                        "</GVPSRequest>";

                    try
                    {

                        var client = new RestClient(strHostAddress);
                        var request = new RestRequest();

                        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                        request.AddParameter("data", strXML);

                        var response = client.Post(request);
                        String content = response.Content;

                        //JSON'a gerek yok.
                        if (content.Contains("<ReasonCode>00</ReasonCode>"))
                        {
                            Yanit.Onay = 1;
                            Yanit.IslemNo = strOrderID;
                            Yanit.Hata = strMDStatusText;
                            Yanit.detay = content;
                            Yanit.Odeme = int.Parse(strAmount) / 100;
                        }
                        else
                        {
                            Yanit.Onay = 0;
                            Yanit.IslemNo = strOrderID;
                            Yanit.Hata = strMDStatusText;
                        }

                    }
                    catch (Exception e)
                    {
                        Yanit.Onay = 0;
                        Yanit.IslemNo = strOrderID;
                        Yanit.Hata = strMDStatusText;
                    }
                }
                else
                {
                    Yanit.Onay = 0;
                    Yanit.IslemNo = strOrderID;
                    Yanit.Hata = strMDStatusText;
                }
            }
            else
            {
                Yanit.Onay = 0;
                Yanit.IslemNo = strOrderID;
                Yanit.Hata = strMDStatusText;
            }

            //Session'dan PosID al
            Yanit.PosID = int.Parse(HttpContext.Current.Session["POSID"].ToString());
            //Session'daki PosID'yi güncelle
            HttpContext.Current.Session["POSID"] = "";

            return Yanit;
        }
    }
}
