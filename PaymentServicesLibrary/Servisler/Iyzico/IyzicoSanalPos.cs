using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Web;
using System.Collections.Specialized;

using Newtonsoft.Json.Linq;

using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;

using PaymentServicesLibrary.Modeller;

namespace PaymentServicesLibrary.Servisler.Iyzico
{
    public class IyzicoSanalPos
    {

        public String PosIyzipay(Pos Cpos, KKart Ckkart, Kullanici Ckullanici, PosIslem CPosIslem)
        {
            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            ThreedsInitialize threedsInitialize;

            Options options = new Options();
            options.ApiKey = PosBilgi[0];
            options.SecretKey = PosBilgi[1];
            options.BaseUrl = "https://sandbox-api.iyzipay.com";


            CreatePaymentRequest requestIni = new CreatePaymentRequest();
            requestIni.Price = CPosIslem.Odeme;
            requestIni.PaidPrice = CPosIslem.Odeme;
            requestIni.Currency = Currency.TRY.ToString();
            requestIni.Installment = int.Parse(CPosIslem.Taksit);
            requestIni.CallbackUrl = GenelMetotlar.getDomain() + "/" + Cpos.PosDonus;

            PaymentCard card = new PaymentCard();
            card.CardNumber = Ckkart.KartNo;
            card.ExpireMonth = Ckkart.SonKulAy;
            card.ExpireYear = Ckkart.SonKulYil;
            card.Cvc = Ckkart.cvc2;
            card.CardHolderName = Ckullanici.KullaniciAdi;

            requestIni.PaymentCard = card;


            String[] isimler = Ckullanici.KullaniciAdi.Split(' ');
            String soyisim = isimler[isimler.Length - 1];

            String isim = "";

            for (int i = 0; i < isimler.Length - 1; i++)
            {
                isim += isimler[i] + " ";
            }
            isim = isim.Substring(0, isim.Length - 1);

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = isim;
            buyer.Surname = soyisim;
            buyer.Email = Ckullanici.KullaniciEmail;
            buyer.IdentityNumber = "00000000";
            buyer.RegistrationAddress = "Default Address";
            buyer.Ip = GenelMetotlar.getIP();
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            requestIni.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = Ckullanici.KullaniciAdi;
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Default Address";
            shippingAddress.ZipCode = "34742";
            requestIni.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = Ckullanici.KullaniciAdi;
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Default Address";
            billingAddress.ZipCode = "34742";
            requestIni.BillingAddress = billingAddress;

            /* Önce en az bir alt üye işyeri tanımlanmalı.
             * 
            CreateSubMerchantRequest request = new CreateSubMerchantRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.SubMerchantExternalId = "B493457635624";
            request.SubMerchantType = SubMerchantType.PERSONAL.ToString();
            request.Address = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            request.ContactName = "John";
            request.ContactSurname = "Doe";
            request.Email = "email@submerchantemail.com";
            request.GsmNumber = "+905350000000";
            request.Name = "John's market";
            request.Iban = "TR180006200119000006672315";
            request.IdentityNumber = "31300864726";
            request.Currency = Currency.TRY.ToString();

            SubMerchant subMerchant = SubMerchant.Create(request, options);
            */

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI144";
            firstBasketItem.Name = "Sepet Toplamı";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = CPosIslem.Odeme;
            firstBasketItem.SubMerchantKey = "eBWcrC18LwBvaRk7VN7AbjOIS/g=";
            firstBasketItem.SubMerchantPrice = CPosIslem.Odeme;

            basketItems.Add(firstBasketItem);

            requestIni.BasketItems = basketItems;

            ServicePointManager.Expect100Continue = true;//SSL-TLS Protokolleri için
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;//SSL-TLS Protokolleri için

            threedsInitialize = ThreedsInitialize.Create(requestIni, options);

            return threedsInitialize.HtmlContent;//ödeme formunu döner.
        }


        public PosYanit PosIyzipayDonus(Pos Cpos)
        {
            String[] PosBilgi = Cpos.PosBilgi.Split('|');

            PosYanit Yanit = new PosYanit();

            NameValueCollection nvc = HttpContext.Current.Request.Form;

            if (nvc["status"].Equals("success"))
            {
                Options options = new Options();
                options.ApiKey = PosBilgi[0];
                options.SecretKey = PosBilgi[1];
                options.BaseUrl = "https://sandbox-api.iyzipay.com";

                RetrievePaymentRequest request = new RetrievePaymentRequest();
                request.ConversationId = "123456789";
                request.PaymentId = nvc["paymentId"];

                Payment payment = Payment.Retrieve(request, options);

                JObject obj = JObject.FromObject(payment);


                if (payment.Status.Equals("success") && payment.FraudStatus.Equals(1))
                {
                    Yanit.Onay = 1;
                    Yanit.IslemNo = payment.PaymentId;
                    Yanit.Hata = "Onaylandı.";
                    Yanit.detay = "";
                    String afterDot = payment.Price.Split('.')[1].Substring(0, 2);
                    Yanit.Odeme = double.Parse(payment.Price.Split('.')[0] + "." + afterDot) / 100;
                }
                else
                {
                    Yanit.Onay = 0;
                    Yanit.IslemNo = "0";
                    Yanit.Hata = payment.ErrorCode + " --> " + payment.ErrorMessage;
                }
            }
            else
            {
                Yanit.Onay = 0;
                Yanit.IslemNo = "0";
                Yanit.Hata = "İşlem başarısız. Hatalı girdi.";
            }

            return Yanit;
        }

    }
}
