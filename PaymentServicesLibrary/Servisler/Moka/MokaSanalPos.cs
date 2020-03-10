using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Collections.Specialized;

using PaymentServicesLibrary.Modeller;
using PaymentServicesLibrary.Servisler.Moka.Modeller;

namespace PaymentServicesLibrary.Servisler.Moka
{
    public class MokaSanalPos
    {
        public String PosMoka3D(Pos Cpos, KKart Ckkart, Kullanici Ckullanici, PosIslem CPosIslem)
        {
            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            String cardholder = Ckullanici.KullaniciAdi;
            String cardnumber = Ckkart.KartNo;
            String expYear = "20" + Ckkart.SonKulYil;
            String expMonth = Ckkart.SonKulAy;
            String cvv = Ckkart.cvc2;
            String amount = CPosIslem.Odeme;

            DealerPaymentServicePaymentRequest request = new DealerPaymentServicePaymentRequest();
            request.PaymentDealerAuthentication = new PaymentDealerAuthentication();


            request.PaymentDealerAuthentication.DealerCode = PosBilgi[0];
            request.PaymentDealerAuthentication.Username = PosBilgi[1];
            request.PaymentDealerAuthentication.Password = PosBilgi[2];


            request.PaymentDealerAuthentication.CheckKey = GenelMetotlar.SHA256Hash(request.PaymentDealerAuthentication.DealerCode + "MK" + request.PaymentDealerAuthentication.Username + "PD" + request.PaymentDealerAuthentication.Password);

            request.PaymentDealerRequest = new PaymentDealerRequest();
            request.PaymentDealerRequest.CardHolderFullName = cardholder;
            request.PaymentDealerRequest.CardNumber = cardnumber;
            request.PaymentDealerRequest.ExpMonth = expMonth;
            request.PaymentDealerRequest.ExpYear = expYear;
            request.PaymentDealerRequest.CvcNumber = cvv;
            request.PaymentDealerRequest.Amount = (decimal)double.Parse(amount);
            request.PaymentDealerRequest.Currency = "TL";
            request.PaymentDealerRequest.InstallmentNumber = 1;
            request.PaymentDealerRequest.IsPreAuth = 0;
            request.PaymentDealerRequest.OtherTrxCode = Guid.NewGuid().ToString();
            HttpCookie trxCookie = new HttpCookie("OtherTrxCode");
            trxCookie.Value = request.PaymentDealerRequest.OtherTrxCode;
            trxCookie.Expires = DateTime.Now.AddMinutes(3.0);
            HttpContext.Current.Response.Cookies.Add(trxCookie);

            if (HttpContext.Current.Request.UserHostAddress.Equals("::1"))
            {
                request.PaymentDealerRequest.ClientIP = "46.197.222.101";
            }
            else
            {
                request.PaymentDealerRequest.ClientIP = HttpContext.Current.Request.UserHostAddress;
            }
            request.PaymentDealerRequest.RedirectUrl = GenelMetotlar.getDomain() + "/" + Cpos.PosDonus;

            //Test
            //string postUrl = "https://service.testmoka.com/PaymentDealer/DoDirectPaymentThreeD";

            //Reel
            string postUrl = "https://service.moka.com/PaymentDealer/DoDirectPaymentThreeD";


            var httpWebRequest = (HttpWebRequest)WebRequest.Create(postUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(request);
                streamWriter.Write(json);
            }

            DealerPaymentServicePaymentResult dealerPaymentServicePaymentResult;

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();
                dealerPaymentServicePaymentResult = new JavaScriptSerializer().Deserialize<DealerPaymentServicePaymentResult>(result);
            }
            if (dealerPaymentServicePaymentResult.ResultCode.Equals("Success"))
            {

                string redirectUrl = dealerPaymentServicePaymentResult.Data;
                //resultPlace.InnerHtml = "İşlem başarılı. 3D için tıklayın.<br />";
                //resultPlace.InnerHtml += "<a href=\"" + redirectUrl + "\" >3D Secure</a>";

                String formContent = "<meta http-equiv = \"refresh\" content = \"0; url = " + redirectUrl + "\" />";

                return formContent;
            }
            else
            {
                String message = "";
                if (dealerPaymentServicePaymentResult.ResultCode.Equals("PaymentDealer.DoDirectPayment3dRequest.InstallmentNotAvailableForForeignCurrencyTransaction"))
                {
                    message = "Yabancı para işlemlerinde taksit işlemi uygulanamaz!";
                }
                else if (dealerPaymentServicePaymentResult.ResultCode.Equals("PaymentDealer.DoDirectPayment3dRequest.ThisInstallmentNumberNotAvailableForDealer"))
                {
                    message = "Seçtiğiniz taksit bayi hesabınızda tanımlı değildir!";
                }
                else if (dealerPaymentServicePaymentResult.ResultCode.Equals("PaymentDealer.DoDirectPayment3dRequest.ForeignCurrencyNotAvailableForThisDealer"))
                {
                    message = "Yabancı para işlemleri bayi tanımınızda tanımlı değildir!";
                }
                else if (dealerPaymentServicePaymentResult.ResultCode.Equals("PaymentDealer.DoDirectPayment3dRequest.ThisInstallmentNumberNotAvailableForVirtualPos"))
                {
                    message = "Bu taksit sayısı seçili sanal pos için kullanılamaz!";
                }
                else if (dealerPaymentServicePaymentResult.ResultCode.Equals("PaymentDealer.CheckDealerPaymentLimits.DailyDealerLimitExceeded"))
                {
                    message = "Bayi limit aşımı nedeniyle işleminizi gerçekleştiremiyoruz. Lütfen ilgili birimimizle irtibata geçiniz.";
                }
                else
                {
                    message = "Error: " + dealerPaymentServicePaymentResult.ResultCode;
                }

                String formContent = "<form id=\"parameters\" method=\"post\" action=\"" + request.PaymentDealerRequest.RedirectUrl + "\" >" +
                    "<input type=\"hidden\" name=\"HataMesaji\" value=\"" + message + "\" />" +
                    "</form>" +
                    "<script type=\"text/javascript\">document.getElementById(\"parameters\").submit();</script>";

                return formContent;
            }

        }



        public PosYanit PosMokaDonus(Pos Cpos)
        {


            PosYanit Yanit = new PosYanit();

            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            NameValueCollection nvc = HttpContext.Current.Request.Form;

            if (nvc["HataMesaji"] != null)
            {
                Yanit.Onay = 0;
                Yanit.IslemNo = "0";
                Yanit.Hata = nvc["HataMesaji"];
            }
            else
            {
                String isSuccessful = nvc["isSuccessful"];
                String resultCode = nvc["resultCode"];
                String resultMessage = nvc["resultMessage"];
                String trxCode = nvc["trxCode"];

                String userMessage = "";


                DealerPaymentServicePaymentRequest request = new DealerPaymentServicePaymentRequest();
                request.PaymentDealerAuthentication = new PaymentDealerAuthentication();

                request.PaymentDealerAuthentication.DealerCode = PosBilgi[0];
                request.PaymentDealerAuthentication.Username = PosBilgi[1];
                request.PaymentDealerAuthentication.Password = PosBilgi[2];


                request.PaymentDealerAuthentication.CheckKey = GenelMetotlar.SHA256Hash(request.PaymentDealerAuthentication.DealerCode + "MK" + request.PaymentDealerAuthentication.Username + "PD" + request.PaymentDealerAuthentication.Password);

                request.PaymentDealerRequest = new PaymentDealerRequest();
                request.PaymentDealerRequest.OtherTrxCode = HttpContext.Current.Request.Cookies.Get("OtherTrxCode").Value;

                string postUrl = "https://service.moka.com/PaymentDealer/GetDealerPaymentTrxDetailList";


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(postUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(request);
                    streamWriter.Write(json);
                }

                DealerPaymentServiceDirectPaymentResult dealerPaymentServicePaymentResult;

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    dealerPaymentServicePaymentResult = new JavaScriptSerializer().Deserialize<DealerPaymentServiceDirectPaymentResult>(result);
                }

                if (dealerPaymentServicePaymentResult.ResultCode.Equals("Success"))
                {
                    if (dealerPaymentServicePaymentResult.Data.PaymentDetail.TrxStatus == 1)
                    {
                        PaymentDetail dt = dealerPaymentServicePaymentResult.Data.PaymentDetail;

                        Yanit.Onay = 1;
                        Yanit.IslemNo = dt.OtherTrxCode;
                        Yanit.Hata = "Odeme basarili!";
                        Yanit.detay = "";
                        Yanit.Odeme = (double)dt.Amount;
                    }
                    else
                    {
                        TrxDetail dt = dealerPaymentServicePaymentResult.Data.PaymentTrxDetailList[0];

                        Yanit.Onay = 0;
                        Yanit.IslemNo = "0";
                        Yanit.Hata = dt.ResultMessage;
                    }
                }
                else
                {
                    if (resultMessage.Equals("PaymentDealer.CheckPaymentDealerAuthentication.InvalidRequest"))
                    {
                        userMessage = "Hatalı hash bilgisi.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.CheckPaymentDealerAuthentication.InvalidAccount"))
                    {
                        userMessage = "Böyle bir bayi bulunamadı.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.CheckPaymentDealerAuthentication.VirtualPosNotFound"))
                    {
                        userMessage = "Bu bayi için sanal pos tanımı yapılmamış.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.CheckDealerPaymentLimits.DailyDealerLimitExceeded"))
                    {
                        userMessage = "Bayi için tanımlı günlük limitlerden herhangi biri aşıldı.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.CheckDealerPaymentLimits.DailyCardLimitExceeded"))
                    {
                        userMessage = "Gün içinde bu kart kullanılarak daha fazla işlem yapılamaz.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.CheckCardInfo.InvalidCardInfo"))
                    {
                        userMessage = "Kart bilgilerinde hata var.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.InvalidRequest"))
                    {
                        userMessage = "JSON objesi yanlış oluşturulmuş.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.RedirectUrlRequired"))
                    {
                        userMessage = "3D ödeme sonucunun döneceği RedirectURL verilmemiş.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.InvalidCurrencyCode"))
                    {
                        userMessage = "Para birimi hatalı. (TL, USD, EUR şeklinde olmalı)";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.InvalidInstallmentNumber"))
                    {
                        userMessage = "Geçersiz taksit sayısı girilmiş 1-12 arası olmalı.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.InstallmentNotAvailableForForeignCurrencyTransaction"))
                    {
                        userMessage = "Yabancı para ile taksit yapılamaz.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.ForeignCurrencyNotAvailableForThisDealer"))
                    {
                        userMessage = "Bayinin yabancı parayla ödeme izni yok.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.PaymentMustBeAuthorization"))
                    {
                        userMessage = "Ön otorizasyon tipinde ödeme gönderilmeli.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.AuthorizationForbiddenForThisDealer"))
                    {
                        userMessage = "Bayinin ön otorizasyon tipinde ödeme gönderme izni yok.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.PoolPaymentNotAvailableForDealer"))
                    {
                        userMessage = "Bayinin havuzlu ödeme gönderme izni yok.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.PoolPaymentRequiredForDealer"))
                    {
                        userMessage = "Bayi sadece havuzlu ödeme gönderebilir.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.TokenizationNotAvailableForDeale"))
                    {
                        userMessage = "Bayinin kart saklama izni yok.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.CardTokenCannotUseWithSaveCard"))
                    {
                        userMessage = "Kart saklanmak isteniyorsa Token gönderilemez.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.CardTokenNotFound"))
                    {
                        userMessage = "Gönderilen Token bulunamadı.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.OnlyCardTokenOrCardNumber"))
                    {
                        userMessage = "Hem kart numarası hem de Token aynı anda verilemez.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.ChannelPermissionNotAvailable"))
                    {
                        userMessage = "Bayinin bu kanaldan ödeme gönderme izni yok.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.IpAddressNotAllowed"))
                    {
                        userMessage = "Bayinin IP kısıtlaması var, sadece önceden belirtilen IP den ödeme gönderebilir.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.VirtualPosNotAvailable"))
                    {
                        userMessage = "Girilen kart için uygun sanal pos bulunamadı.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.ThisInstallmentNumberNotAvailableForVirtualPos"))
                    {
                        userMessage = "Sanal Pos bu taksit sayısına izin vermiyor.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.ThisInstallmentNumberNotAvailableForDealer"))
                    {
                        userMessage = "Bu taksit sayısı bu bayi için yapılamaz.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.DealerCommissionRateNotFound"))
                    {
                        userMessage = "Bayiye bu sanal pos ve taksit için komisyon oranı girilmemiş.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.DealerGroupCommissionRateNotFound"))
                    {
                        userMessage = "Üst bayiye bu sanal pos ve taksit için komisyon oranı girilmemiş.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.InvalidSubMerchantName"))
                    {
                        userMessage = "Gönderilen bayi adı daha önceden Moka sistemine kaydedilmemiş.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.InvalidUnitPrice"))
                    {
                        userMessage = "Satılan ürünler sepete eklendiyse, geçerli birim fiyatı girilmelidir.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.InvalidQuantityValue"))
                    {
                        userMessage = "Satılan ürünler sepete eklendiyse, geçerli adet girilmelidir.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.BasketAmountIsNotEqualPaymentAmount"))
                    {
                        userMessage = "Satılan ürünler sepete eklendiyse, sepet tutarı ile ödeme tutarı eşleşmelidir.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.BasketProductNotFoundInYourProductList"))
                    {
                        userMessage = "Satılan ürünler sepete eklendiyse, geçerli ürün seçilmelidir.";
                    }
                    else if (resultMessage.Equals("PaymentDealer.DoDirectPayment3dRequest.MustBeOneOfDealerProductIdOrProductCode"))
                    {
                        userMessage = "Satılan ürünler sepete eklendiyse, ürün kodu veya moka ürün ID si girilmelidir.";
                    }
                    else if (resultMessage.Equals("EX"))
                    {
                        userMessage = "Beklenmeyen bir hata oluştu.";
                    }
                    else
                    {
                        userMessage = resultMessage;
                    }

                    Yanit.Onay = 0;
                    Yanit.IslemNo = "0";
                    Yanit.Hata = userMessage;

                }



            }

            return Yanit;
        }
    }
}
