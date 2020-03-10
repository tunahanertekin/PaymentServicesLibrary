using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using System.Xml;
using System.Web;
using System.Data.SqlClient;

using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using PaymentServicesLibrary.Modeller;
using PaymentServicesLibrary.IsBankasi.Modeller;

namespace PaymentServicesLibrary.IsBankasi
{
    public class IsBankasiSanalPos
    {
        public String PosIsbank3D(Pos Cpos, KKart Ckkart, Kullanici Ckullanici, PosIslem CPosIslem)
        {
            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            //Dışarıdan al
            var client = new RestClient("https://mpi.vpos.isbank.com.tr/Enrollment.aspx");
            var request = new RestRequest();

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            String islemNo = Guid.NewGuid().ToString();
            String skt = Ckkart.SonKulYil + Ckkart.SonKulAy;
            String taksit = CPosIslem.Taksit;

            if (CPosIslem.Taksit.Equals("0"))
            {
                taksit = "";
            }
            else
            {
                taksit = CPosIslem.Taksit;
            }

            String tutarStr = "";
            int tutar = int.Parse(CPosIslem.Odeme);
            int kurus = tutar % 100;
            String kurusStr = "";
            if (kurus < 10)
            {
                kurusStr = "0" + kurus;
            }
            else
            {
                kurusStr = "" + kurus;
            }
            tutar = tutar / 100;

            tutarStr = tutar + "." + kurusStr;
            


            //Dışarıdan al
            request.AddParameter("MerchantId", PosBilgi[0]);
            request.AddParameter("MerchantPassword", PosBilgi[1]);
            request.AddParameter("VerifyEnrollmentRequestId", islemNo);
            request.AddParameter("Pan", Ckkart.KartNo);
            request.AddParameter("ExpiryDate", skt);
            request.AddParameter("PurchaseAmount", tutarStr);
            request.AddParameter("Currency", "949");
            request.AddParameter("BrandName", "200");
            request.AddParameter("SuccessUrl", GenelMetotlar.getDomain() + "/" + Cpos.PosDonus);
            request.AddParameter("FailureUrl", GenelMetotlar.getDomain() + "/" + Cpos.PosDonus);
            request.AddParameter("MerchantType", "0");

            var response = client.Post(request);
            String content = response.Content;

            //content = content.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);

            //Daima JSON
            String json = JsonConvert.SerializeXmlNode(doc);
            JObject jsonObj = JObject.Parse(json);

            Enrollment er = jsonObj.ToObject<Enrollment>();


            String necessaryData = Ckkart.KartNo + "&" + skt + "&" + Ckkart.cvc2 + "&" + PosBilgi[1] + "&" + PosBilgi[0] + "&" + taksit + "&" + tutarStr + "&" + "949";


            String conString = "Server=185.28.60.253,8081;Database=_tunahan;User Id=tunaHan;Password=TnHan8473;";
            SqlConnection con = new SqlConnection(conString);

            con.Open();

            String query = "INSERT INTO [Hassas] VALUES('" + islemNo + "', '" + necessaryData + "');";
            SqlCommand command = new SqlCommand(query, con);
            command.ExecuteReader();
            
            con.Close();

            String formContent = "";

            try
            {
                if (er.IPaySecure.VERes.Status.Equals("Y"))
                {
                    formContent += "<form id=\"parameters\" method=\"POST\" action=\"" + er.IPaySecure.VERes.ACSUrl + "\">" +
                   "<input type=\"hidden\" name=\"PaReq\" value=\"" + er.IPaySecure.VERes.PAReq + "\" >" +
                   "<input type=\"hidden\" name=\"TermUrl\" value=\"" + er.IPaySecure.VERes.TermUrl + "\" >" +
                   "<input type=\"hidden\" name=\"MD\" value=\"" + er.IPaySecure.VERes.MD + "\" >" +
                   "</form>" +
                   "<script type=\"text/javascript\">document.getElementById(\"parameters\").submit();</script>";
                }
                else
                {
                    HttpContext.Current.Response.Redirect("odemekk.aspx?" + CPosIslem.EKBilgiler.Replace("|","&"));
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Redirect("odemekk.aspx?" + CPosIslem.EKBilgiler.Replace("|", "&"));
            }

            return formContent;
        }



        public PosYanit PosIsbank3DDonus(Pos Cpos)
        {
            String url = HttpContext.Current.Request.Url.AbsoluteUri;
            PosYanit Yanit = new PosYanit();

            String conString = "Server=185.28.60.253,8081;Database=_tunahan;User Id=tunaHan;Password=TnHan8473;";
            SqlConnection con = new SqlConnection(conString);

            con.Open();

            // PARAMETRELERİ VERİTABANINDAN AL
            String query = "SELECT * FROM [Hassas] WHERE id='" + HttpContext.Current.Request.Form["VerifyEnrollmentRequestId"] + "';";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader result = command.ExecuteReader();
            result.Read();
            String sessionData = result["collective_parameters"].ToString();
            result.Close();

            // PARAMETRELERİ VERİTABANINDAN SİL
            query = "DELETE FROM [Hassas] WHERE id='" + HttpContext.Current.Request.Form["VerifyEnrollmentRequestId"] + "'";
            command = new SqlCommand(query, con);
            result = command.ExecuteReader();

            con.Close();


            //String sessionData = HttpContext.Current.Session["necessaryData"].ToString();
            String[] sessionArr = sessionData.Split('&');

            String PostUrl = "https://trx.vpos.isbank.com.tr/VposWeb/v3/Vposreq.aspx";
            String IsyeriNo = sessionArr[4];
            String IsyeriSifre = sessionArr[3];
            String KartNo = sessionArr[0];
            String KartExpiry = "20" + sessionArr[1];
            String KartCvv = sessionArr[2];
            String Tutar = sessionArr[6];
            String SiparID = Guid.NewGuid().ToString();
            String IslemTipi = "Sale";
            String TutarKodu = sessionArr[7];
            String ClientIp =   GenelMetotlar.getIP();


            if (HttpContext.Current.Request.Form["Status"].Equals("Y"))
            {
                String PosXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<VposRequest>" +
                    "<ECI>" + HttpContext.Current.Request.Form["Eci"] + "</ECI>" +
                    "<CAVV>" + HttpContext.Current.Request.Form["Cavv"] + "</CAVV>" +
                    "<MpiTransactionId>" + HttpContext.Current.Request.Form["VerifyEnrollmentRequestId"] + "</MpiTransactionId>" +
                    "<MerchantId>" + IsyeriNo + "</MerchantId>" +
                    "<Password>" + IsyeriSifre + "</Password>" +
                    "<TransactionType>" + IslemTipi + "</TransactionType>" +
                    "<TransactionId>" + SiparID + "</TransactionId>" +
                    "<CurrencyAmount>" + Tutar + "</CurrencyAmount>" +
                    "<CurrencyCode>" + TutarKodu + "</CurrencyCode>" +
                    "<Pan>" + KartNo + "</Pan>" +
                    "<Cvv>" + KartCvv + "</Cvv>" +
                    "<Expiry>" + KartExpiry + "</Expiry>" +
                    "<TransactionDeviceSource>0</TransactionDeviceSource>" +
                    "<ClientIp>" + ClientIp + "</ClientIp>" +
                    "</VposRequest>";

                //Dışarıdan al
                var client = new RestClient(PostUrl);
                var request = new RestRequest();

                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                request.AddParameter("prmstr", PosXML);

                var response = client.Post(request);
                String content = response.Content;

                //content = content.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(content);

                //Daima JSON
                String json = JsonConvert.SerializeXmlNode(doc);
                JObject jsonObj = JObject.Parse(json);

                //Enrollment er = jsonObj.ToObject<Enrollment>();
                IsbankResponse ir = jsonObj.ToObject<IsbankResponse>();



                if (ir.VposResponse.ResultCode.Equals("0000"))
                {
                    Yanit.Onay = 1;
                    Yanit.IslemNo = ir.VposResponse.AuthCode;
                    Yanit.Hata = "";
                    Yanit.detay = Tutar.Replace(".",",") + " " + ir.VposResponse.CurrencyCode;
                    Yanit.Odeme = double.Parse(Tutar)/100;
                    Yanit.Yanit = ir.VposResponse.ResultDetail;
                }
                else
                {
                    Yanit.Onay = 0;
                    Yanit.IslemNo = "0";
                    Yanit.Yanit = ir.VposResponse.ResultDetail;
                    try
                    {
                        Yanit.Hata = ir.VposResponse.ResultDetail.Replace("'","");
                        // veritabanından hata kodunu al
                    }
                    catch (Exception e)
                    {
                        Yanit.Hata = "İşlemde hata oluştu.";
                    }
                }
            }
            else
            {
                Yanit.Onay = 0;
                Yanit.IslemNo = "0";
                Yanit.Hata = "İşlemde hata var.";
                Yanit.Yanit = "İşlem başarısız.";

            }


            /*
            Yanit.PosID = int.Parse(HttpContext.Current.Session["POSID"].ToString());
            HttpContext.Current.Session["POSID"] = "";
            HttpContext.Current.Session["EkBilgiler"] = "";
            */
            //Session'daki POSID ve EkBilgiler parametrelerini güncelle.

            return Yanit;
        }
    }
}