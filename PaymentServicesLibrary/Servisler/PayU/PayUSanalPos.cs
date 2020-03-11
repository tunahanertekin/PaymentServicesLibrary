using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Collections.Specialized;

using Newtonsoft.Json.Linq;

using PaymentServicesLibrary.Modeller;

using PaymentServicesLibrary.Servisler.PayU.Modeller;
using PaymentServicesLibrary.Servisler.PayU.Modeller.Request;
using PaymentServicesLibrary.Servisler.PayU.Modeller.Response;

namespace PaymentServicesLibrary.Servisler.PayU
{
    public class PayUSanalPos
    {
        public String PosPayU(Pos Cpos, KKart Ckkart, Kullanici Ckullanici, PosIslem CPosIslem)
        {
            String[] PosBilgi = Cpos.PosBilgi.Split('$');

            String cardholder = Ckullanici.KullaniciAdi;
            String cardnumber = Ckkart.KartNo;
            String expMonth = Ckkart.SonKulAy;
            String expYear = Ckkart.SonKulYil;
            String cvv = Ckkart.cvc2;
            String paymentType = "3D";

            ApiPaymentRequest apiPaymentRequest = new ApiPaymentRequest();

            apiPaymentRequest.Config = new ApiPaymentRequest.PayUConfig();


            if (paymentType.Equals("non3D"))
            {
                apiPaymentRequest.Config.MERCHANT = "OPU_TEST";
            }
            else if (paymentType.Equals("3D"))
            {
                apiPaymentRequest.Config.MERCHANT = PosBilgi[0];
            }
            apiPaymentRequest.Config.LANGUAGE = "TR";
            apiPaymentRequest.Config.PAY_METHOD = "CCVISAMC";
            apiPaymentRequest.Config.BACK_REF = GenelMetotlar.getDomain() + "/" + Cpos.PosDonus;
            apiPaymentRequest.Config.PRICES_CURRENCY = "TRY";
            apiPaymentRequest.Order = new ApiPaymentRequest.PayUOrder();
            apiPaymentRequest.Order.ORDER_REF = Guid.NewGuid().ToString();
            apiPaymentRequest.Order.ORDER_SHIPPING = "0";
            apiPaymentRequest.Order.ORDER_DATE = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            ApiPaymentRequest.PayUOrder.PayUOrderItem orderItem = new ApiPaymentRequest.PayUOrder.PayUOrderItem();
            orderItem.ORDER_PRICE = CPosIslem.Odeme;
            orderItem.ORDER_PINFO = "Test Açıklaması";
            orderItem.ORDER_QTY = "1";
            orderItem.ORDER_PCODE = "Test Kodu";
            orderItem.ORDER_PNAME = "Test Ürünü";
            orderItem.ORDER_VAT = "0";
            orderItem.ORDER_PRICE_TYPE = "NET";

            apiPaymentRequest.Order.OrderItems.Add(orderItem);

            apiPaymentRequest.CreditCard = new ApiPaymentRequest.PayUCreditCard();

            apiPaymentRequest.CreditCard.CC_NUMBER = cardnumber;
            apiPaymentRequest.CreditCard.EXP_MONTH = expMonth;
            apiPaymentRequest.CreditCard.EXP_YEAR = expYear;
            apiPaymentRequest.CreditCard.CC_CVV = cvv;
            apiPaymentRequest.CreditCard.CC_OWNER = cardholder;
            apiPaymentRequest.CreditCard.SELECTED_INSTALLMENTS_NUMBER = "1";

            //Fatura
            apiPaymentRequest.Customer = new ApiPaymentRequest.PayUCustomer();
            apiPaymentRequest.Customer.BILL_FNAME = "Ad";
            apiPaymentRequest.Customer.BILL_LNAME = "Soyad";
            apiPaymentRequest.Customer.BILL_EMAIL = "mail@mail.com";
            apiPaymentRequest.Customer.BILL_PHONE = "02129003711";
            apiPaymentRequest.Customer.BILL_FAX = "02129003711";
            apiPaymentRequest.Customer.BILL_ADDRESS = "Birinci Adres satırı";
            apiPaymentRequest.Customer.BILL_ADDRESS2 = "İkinci Adres satırı";
            apiPaymentRequest.Customer.BILL_ZIPCODE = "34000";
            apiPaymentRequest.Customer.BILL_CITY = "ISTANBUL";
            apiPaymentRequest.Customer.BILL_COUNTRYCODE = "TR";
            apiPaymentRequest.Customer.BILL_STATE = "Ayazağa";
            apiPaymentRequest.Customer.CLIENT_IP = HttpContext.Current.Request.UserHostAddress;

            //Kargo
            apiPaymentRequest.Delivery = new ApiPaymentRequest.PayUDelivery();
            apiPaymentRequest.Delivery.DELIVERY_FNAME = "Ad";
            apiPaymentRequest.Delivery.DELIVERY_LNAME = "Soyad";
            apiPaymentRequest.Delivery.DELIVERY_EMAIL = "mail@mail.com";
            apiPaymentRequest.Delivery.DELIVERY_PHONE = "02129003711";
            apiPaymentRequest.Delivery.DELIVERY_COMPANY = "PayU Ödeme Kuruluşu A.Ş.";
            apiPaymentRequest.Delivery.DELIVERY_ADDRESS = "Birinci Adres satIRI";
            apiPaymentRequest.Delivery.DELIVERY_ADDRESS2 = "İkinci Adres satırı";
            apiPaymentRequest.Delivery.DELIVERY_ZIPCODE = "34000";
            apiPaymentRequest.Delivery.DELIVERY_CITY = "ISTANBUL";
            apiPaymentRequest.Delivery.DELIVERY_STATE = "TR";
            apiPaymentRequest.Delivery.DELIVERY_COUNTRYCODE = "Ayazağa";

            var options = new Options();
            options.Url = "https://secure.payu.com.tr/order/alu/v3";

            if (paymentType.Equals("non3D"))
            {
                options.SecretKey = "SECRET_KEY";
            }
            else if (paymentType.Equals("3D"))
            {
                options.SecretKey = PosBilgi[1];
            }

            ApiPaymentNon3DResponse responseNon3D;
            ApiPayment3DResponse response3D;

            String formContent = "";

            if (paymentType.Equals("non3D"))
            {
                responseNon3D = ApiPaymentRequest.Non3DExecute(apiPaymentRequest, options);
                JObject responseJson = JObject.FromObject(responseNon3D);
                //responsePlace.InnerHtml = responseJson.ToString(Formatting.Indented);
                formContent = "3D'siz Ödeme Deneniyor!!!";
            }
            else if (paymentType.Equals("3D"))
            {
                response3D = ApiPaymentRequest.ThreeDSecurePayment(apiPaymentRequest, options);
                JObject responseJson = JObject.FromObject(response3D);


                if (response3D.URL_3DS == null || response3D.URL_3DS.Equals(""))
                {
                    formContent = "<form id=\"parameters\" method=\"post\" action=\"" + apiPaymentRequest.Config.BACK_REF + "\" >" +
                    "<input type=\"hidden\" name=\"HataMesaji\" value=\"" + "hataligirdi" + "\" />" +
                    "</form>" +
                    "<script type=\"text/javascript\">document.getElementById(\"parameters\").submit();</script>";
                }
                else
                {
                    formContent = "<meta http-equiv = \"refresh\" content = \"0; url = " + response3D.URL_3DS + "\" />";
                }
            }

            return formContent;
        }

        public PosYanit PosPayUDonus(Pos Cpos)
        {
            PosYanit Yanit = new PosYanit();

            NameValueCollection nvc = HttpContext.Current.Request.Form;

            if (nvc["HataMesaji"] != null)
            {
                Yanit.Onay = 0;
                Yanit.IslemNo = "0";
                Yanit.Hata = "Hatalı girdi!";
            }
            else
            {
                String mdStatus = nvc["MDSTATUS"];
                String status = nvc["STATUS"];
                String resultCode = nvc["RETURN_CODE"];

                String userMessage = "";


                if (status.Equals("SUCCESS") && resultCode.Equals("AUTHORIZED") && mdStatus.Equals("1"))
                {
                    Yanit.Onay = 1;
                    Yanit.IslemNo = nvc["ORDER_REF"];
                    Yanit.Hata = "Onaylandı.";
                    Yanit.detay = "";
                    Yanit.Odeme = double.Parse(nvc["AMOUNT"]);
                }
                else
                {
                    if (mdStatus.Equals("0"))
                    {
                        userMessage = "3-D Secure imzası geçersiz, onaylanmamış.";
                    }
                    else if (mdStatus.Equals("1"))
                    {
                        userMessage = "3-D ile Onaylanmış.";
                    }
                    else if (mdStatus.Equals("2"))
                    {
                        userMessage = "Kart sahibi veya bankası sisteme kayıtlı değil.";
                    }
                    else if (mdStatus.Equals("3"))
                    {
                        userMessage = "Kartın bankası sisteme kayıtlı değil.";
                    }
                    else if (mdStatus.Equals("4"))
                    {
                        userMessage = "Doğrulama denemesi, kart sahibi sisteme daha sonra kayıt olmayı seçmiş.";
                    }
                    else if (mdStatus.Equals("5"))
                    {
                        userMessage = "Doğrulama yapılamıyor, sistem arızası.";
                    }
                    else if (mdStatus.Equals("6"))
                    {
                        userMessage = "3-D Secure hatası.";
                    }
                    else if (mdStatus.Equals("7"))
                    {
                        userMessage = "Sistem hatası.";
                    }
                    else if (mdStatus.Equals("8"))
                    {
                        userMessage = "Kart geçersiz, bilinmeyen kart.";
                    }
                    else if (mdStatus.Equals("9"))
                    {
                        userMessage = "Üye İşyeri/POS 3D-Secure sistemine kayıtlı değil.";
                    }
                    else
                    {
                        userMessage = "Beklenmedik hata.";
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
