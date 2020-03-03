using System;
using System.Collections.Generic;
using System.Text;

using System.Security.Cryptography;
using System.Web;

using PaymentServicesLibrary.Modeller;

namespace PaymentServicesLibrary.Akbank
{
    public class AkbankSanalPos
    {

        public String PosESTAkbank(Pos Cpos, KKart Ckkart, Kullanici Ckullanici, PosIslem CPosIslem)
        {
            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            String clientid = PosBilgi[0];
            String amount = CPosIslem.Odeme;
            String oid = Guid.NewGuid().ToString().Replace("-", "_");
            String okUrl = GenelMetotlar.getDomain() + "/" + Cpos.PosDonus;
            String failUrl = GenelMetotlar.getDomain() + "/" + Cpos.PosDonus;
            String rnd = DateTime.Now.ToString();
            String taksit = "";
            if (CPosIslem.Taksit.Equals("0"))
            {
                taksit = "0";
            }
            else
            {
                taksit = "0" + CPosIslem.Taksit;
            }
            String islemtipi = "Auth";
            String storeKey = PosBilgi[1];
            String hashstr = clientid + oid + amount + okUrl + failUrl + islemtipi + taksit + rnd + storeKey;

            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(hashstr);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            String hash = Convert.ToBase64String(inputbytes);

            String pan = Ckkart.KartNo;
            String Ecom_Payment_Card_ExpDate_Month = Ckkart.SonKulAy;
            String Ecom_Payment_Card_ExpDate_Year = Ckkart.SonKulYil;
            String cv2 = Ckkart.cvc2;

            String formContent = "<form id=\"parameters\" method=\"post\" action=\"https://www.sanalakpos.com/fim/est3Dgate\">" +
                "<input type=\"hidden\" id=\"clientid\" name=\"clientid\" value=\"" + clientid + "\" />" +
                "<input type=\"hidden\" id=\"storetype\" name=\"storetype\" value=\"3d_pay\" />" +
                "<input type=\"hidden\" id=\"hash\" name=\"hash\" value=\"" + hash + "\" />" +
                "<input type=\"hidden\" id=\"islemtipi\" name=\"islemtipi\" value=\"" + islemtipi + "\" />" +
                "<input type=\"hidden\" id=\"taksit\" name=\"taksit\" value=\"" + taksit + "\" />" +
                "<input type=\"hidden\" id=\"amount\" name=\"amount\" value=\"" + amount + "\" />" +
                "<input type=\"hidden\" id=\"currency\" name=\"currency\" value=\"949\" />" +
                "<input type=\"hidden\" id=\"oid\" name=\"oid\" value=\"" + oid + "\" />" +
                "<input type=\"hidden\" id=\"okUrl\" name=\"okUrl\" value=\"" + okUrl + "\" />" +
                "<input type=\"hidden\" id=\"failUrl\" name=\"failUrl\" value=\"" + failUrl + "\" />" +
                "<input type=\"hidden\" id=\"lang\" name=\"lang\" value=\"tr\" />" +
                "<input type=\"hidden\" id=\"rnd\" name=\"rnd\" value=\"" + rnd + "\" />" +
                "<input type=\"hidden\" id=\"pan\" name=\"pan\" value=\"" + pan + "\"  />" +
                "<input type=\"hidden\" id=\"Ecom_Payment_Card_ExpDate_Year\" name=\"Ecom_Payment_Card_ExpDate_Year\" value=\"" + Ecom_Payment_Card_ExpDate_Year + "\"  />" +
                "<input type=\"hidden\" id=\"Ecom_Payment_Card_ExpDate_Month\" name=\"Ecom_Payment_Card_ExpDate_Month\" value=\"" + Ecom_Payment_Card_ExpDate_Month + "\"  />";

           

            String[] EkBilgiler = CPosIslem.EKBilgiler.Split('|');
            foreach(String bilgi in EkBilgiler)
            {
                String[] alanlar = bilgi.Split('=');
                formContent += "<input type=\"hidden\" name=\"" + alanlar[0] + "\" value=\"" + alanlar[1] + "\" />";
                //"<input type='hidden' name='" & alanlar(0) & "' value='" & alanlar(1) & "'/>"

            }


            formContent += "</form>" +
            "<script type=\"text/javascript\">document.getElementById(\"parameters\").submit();</script>";

            return formContent;
        }

        public PosYanit PosESTAkbankDonus(Pos Cpos)
        {

            String[] odemeparametreleri = "AuthCode,Response,HostRefNum,ProcReturnCode,TransId,ErrMsg".Split(',');
            var e = HttpContext.Current.Request.Form.GetEnumerator();
            PosYanit Yanit = new PosYanit();
            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            while (e.MoveNext())
            {
                String xkey = e.Current.ToString();
                var xval = HttpContext.Current.Request.Form.Get(xkey);
                Boolean ok = true;

                for (int i = 0; i < odemeparametreleri.Length - 1; i++)
                {
                    if (xkey.Equals(odemeparametreleri[i]))
                    {
                        ok = false;
                        break;
                    }
                }
            }

            String hashparams = HttpContext.Current.Request.Form.Get("HASHPARAMS");
            String hashparamsval = HttpContext.Current.Request.Form.Get("HASHPARAMSVAL");
            String storeKey = PosBilgi[1];
            String paramsval = "";

            foreach (String s in hashparams.Split(':'))
            {
                paramsval += s;
            }

            String hashval = paramsval + storeKey;
            String hashparam = HttpContext.Current.Request.Form.Get("HASH");

            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashbytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(hashval);
            byte[] inputbytes = sha.ComputeHash(hashbytes);
            String hash = Convert.ToBase64String(inputbytes);

            if (!paramsval.Equals(hashparamsval) || !hash.Equals(hashparam))
            {
                Yanit.Onay = 0;
                Yanit.IslemNo = "0";
                Yanit.Hata = HttpContext.Current.Request.Form["ErrMsg"].Replace("'", "");
            }

            String mdStatus = HttpContext.Current.Request.Form.Get("mdStatus");
            if (mdStatus.Equals("1") || mdStatus.Equals("2") || mdStatus.Equals("3") || mdStatus.Equals("4"))
            {
                for (int i = 0; i < odemeparametreleri.Length - 1; i++)
                {
                    String paramname = odemeparametreleri[i];
                    String paramval = HttpContext.Current.Request.Form.Get(paramname);
                }

                if ("Approved".Equals(HttpContext.Current.Request.Form.Get("Response")))
                {
                    Yanit.Onay = 1;
                    Yanit.IslemNo = HttpContext.Current.Request.Form.Get("AuthCode");
                    Yanit.Hata = "Hata yok";
                    Yanit.detay = "";
                    Yanit.Odeme = Double.Parse(HttpContext.Current.Request.Form.Get("amount"));
                }
                else
                {
                    Yanit.Onay = 0;
                    Yanit.IslemNo = "0";
                    Yanit.Hata = HttpContext.Current.Request.Form["ErrMsg"].Replace("'", "");
                }
            }
            else
            {
                Yanit.Onay = 0;
                Yanit.IslemNo = "0";
                Yanit.Hata = HttpContext.Current.Request.Form["ErrMsg"].Replace("'", "");
                //f.bul_deg
            }

            Yanit.PosID = int.Parse(HttpContext.Current.Session["POSID"].ToString());//"Session'daki POSID Parametresi yazılmalı";
            //Session'daki POSID ve EkBilgiler parametreleri değiştirilecek.
            HttpContext.Current.Session["POSID"] = "";
            HttpContext.Current.Session["EkBilgiler"] = "";

            return Yanit;
        }

    }
}
