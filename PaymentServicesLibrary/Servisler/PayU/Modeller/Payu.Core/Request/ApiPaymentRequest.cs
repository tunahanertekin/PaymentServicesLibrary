
using PaymentServicesLibrary.Servisler.PayU.Modeller.Response;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{

    /// <summary>
    /// Bu metod 3d secure ve 3d secure olmadan ödeme için yapılan çağrıların içinde barındırıldığı kısmı temsil etmektedir.
    /// Sınıf içerisinde gerekli parametreler bulunmaktadır.
    /// Sınıf içerisinde Non3DExecute metodu ile 3d secure olmadan ödeme için kullanılan metoddur.
    /// ThreeDSecurePayment metodu ile 3d secure ile ödeme yapılması sağlanmaktadır.
    /// </summary>
    public class ApiPaymentRequest
    {
        public StringBuilder HTML_CONTENT = new StringBuilder();
        public StringBuilder URL_CONTENT = new StringBuilder();
        public StringBuilder XML_CONTENT = new StringBuilder();
        public string HASH_CONTENT = "";
        public string HASHED_CONTENT = "";

        public PayUOrder Order { get; set; }
        public PayUConfig Config { get; set; }
        public PayUCreditCard CreditCard { get; set; }
        public PayUCustomer Customer { get; set; }
        public PayUDelivery Delivery { get; set; }

        public ApiPaymentRequest()
        {
            Order = new PayUOrder();
            Config = new PayUConfig();
            CreditCard = new PayUCreditCard();
            Customer = new PayUCustomer();
            Delivery = new PayUDelivery();
        }

        public class PayUOrder
        {
            public List<PayUOrderItem> OrderItems { get; set; }

            public PayUOrder()
            {
                OrderItems = new List<PayUOrderItem>();
            }

            /// <summary>
            /// Order Reference Number (Optional)
            /// </summary>
            public string ORDER_REF { get; set; }

            public PayUOrder Set_ORDER_REF(string orderRef)
            {
                this.ORDER_REF = orderRef;
                return this;
            }

            public string Get_ORDER_REF()
            {
                return this.ORDER_REF;
            }

            /// <summary>
            /// Order Date (YYYY-MM-DD HH:MM:SS)
            /// </summary>
            public string ORDER_DATE { get; set; }

            public PayUOrder Set_ORDER_DATE(string orderDate)
            {
                string orderDateValue = string.Empty;

                if (orderDate != null)
                {
                    orderDateValue = orderDate;
                }

                this.ORDER_DATE = orderDateValue;

                return this;
            }

            public string Get_ORDER_DATE()
            {
                return this.ORDER_DATE;
            }

            /// <summary>
            /// Global discount for your order. Discount currency is the one used for products prices,
            /// and the value should be positive. (optional)
            /// </summary>
            public string DISCOUNT { get; set; }

            public PayUOrder Set_DISCOUNT(decimal orderDiscountAmount)
            {
                this.DISCOUNT = ConvertDecimalToString(orderDiscountAmount);
                return this;
            }

            public string Get_DISCOUNT()
            {
                return this.DISCOUNT;
            }

            /// <summary>
            /// Order shipping cost.
            /// </summary>
            public string ORDER_SHIPPING { get; set; }

            public PayUOrder Set_ORDER_SHIPPING(decimal orderShippingAmount)
            {
                this.ORDER_SHIPPING = ConvertDecimalToString(orderShippingAmount);
                return this;
            }

            public string Get_ORDER_SHIPPING()
            {
                return this.ORDER_SHIPPING;
            }

            public class PayUOrderItem
            {
                public PayUOrderItem()
                {

                }

                /// <summary>
                /// Product Name. (155 Chars max)
                /// </summary>
                public string ORDER_PNAME { get; set; }

                public string Get_ORDER_PNAME()
                {
                    return Helper.Truncate(this.ORDER_PNAME, 155);
                }

                public PayUOrderItem Set_ORDER_PNAME(string value)
                {
                    this.ORDER_PNAME = Helper.GenerateFriendlyName(value, false);
                    return this;
                }

                /// <summary>
                /// Product Code (20 Chars max)
                /// </summary>
                public string ORDER_PCODE { get; set; }

                public string Get_ORDER_PCODE()
                {
                    return Helper.Truncate(this.ORDER_PCODE, 20);
                }

                public PayUOrderItem Set_ORDER_PCODE(string value)
                {
                    this.ORDER_PCODE = value;
                    return this;
                }

                /// <summary>
                /// Additional information for product (Optional)
                /// </summary>
                public string ORDER_PINFO { get; set; }

                public string Get_ORDER_PINFO()
                {
                    return this.ORDER_PINFO;
                }

                public PayUOrderItem Set_ORDER_PINFO(string value)
                {
                    this.ORDER_PINFO = Helper.GenerateFriendlyName(value, false);
                    return this;
                }

                /// <summary>
                /// Unit price, without VAT, for ordered products. Default currency is RON.
                /// </summary>
                public string ORDER_PRICE { get; set; }

                public string Get_ORDER_PRICE()
                {
                    return this.ORDER_PRICE;
                }

                public PayUOrderItem Set_ORDER_PRICE(decimal value)
                {
                    this.ORDER_PRICE = ConvertDecimalToString(value);
                    return this;
                }

                /// <summary>
                /// Quantities for all ordered products..
                /// </summary>
                public string ORDER_QTY { get; set; }

                public string Get_ORDER_QTY()
                {
                    if (string.IsNullOrEmpty(this.ORDER_QTY))
                    {
                        return "0";
                    }
                    return this.ORDER_QTY;
                }

                public PayUOrderItem Set_ORDER_QTY(int value)
                {
                    this.ORDER_QTY = (value).ToString();
                    return this;
                }


                /// <summary>
                /// the VAT percent, applied to each product.
                /// </summary>
                public string ORDER_VAT { get; set; }

                public string Get_ORDER_VAT()
                {
                    if (string.IsNullOrEmpty(this.ORDER_VAT))
                    {
                        return "0";
                    }

                    return this.ORDER_VAT;
                }

                public PayUOrderItem Set_ORDER_VAT(decimal value)
                {
                    if (value == 0)
                    {
                        this.ORDER_VAT = "0";
                    }
                    else
                    {
                        this.ORDER_VAT = ConvertDecimalToString(value);
                    }

                    return this;
                }

                /// <summary>
                /// the VAT percent, applied to each product.
                /// </summary>
                public string ORDER_PRICE_TYPE { get; set; }

                public string Get_ORDER_PRICE_TYPE()
                {
                    if (string.IsNullOrEmpty(this.ORDER_PRICE_TYPE))
                    {
                        return "BRÜT";
                    }

                    return this.ORDER_PRICE_TYPE;
                }

                public PayUOrderItem Set_ORDER_PRICE_TYPE(string value)
                {
                    this.ORDER_PRICE_TYPE = (value);
                    return this;
                }

            }
        }

        public class PayUConfig
        {
            public PayUConfig()
            {
            }
            public string Set_BACK_REF(string backUrl)
            {
                return backUrl;

            }

            /// <summary>
            /// Merchant Id Given from Pay U
            /// </summary>
            public string MERCHANT { get; set; }

            /// <summary>
            /// Currency used for products and shipping prices. 
            /// Accepted values: RON, EUR, USD or GBP. If this value is not sent, 
            /// default currency will be RON. (optional)
            /// </summary>
            public string PRICES_CURRENCY { get; set; }

            /// <summary>
            /// Accepted values: RON, EUR, USD or GBP. If this value is not sent, 
            /// default currency will be RON. (optional)
            /// </summary>
            public string CURRENCY { get; set; }

            /// <summary>
            /// Payment method for the order (optional). Possible values:
            /// CCVISAMC – Visa/Mastercard credit card (default)
            /// CCAMEX – American Express credit card
            /// CCDINERS – Diners Club Credit Card
            /// CCJCB – JCB Credit Card
            /// WIRE – Wire Transfer
            /// CASH – Cash on Delivery
            /// </summary>
            public string PAY_METHOD { get; set; }

            /// <summary>
            /// Response page where the Gecad will redirect to user in positive response.
            /// </summary>
            public string BACK_REF { get; set; }


            /// <summary>
            /// Language option of the Vpos page
            /// Only Ro and En exists
            /// </summary>
            public string LANGUAGE { get; set; }

            public string LU_ENABLE_TOKEN { get; set; }



            public StringBuilder Validate()
            {
                StringBuilder errors = new StringBuilder();

                if (string.IsNullOrEmpty(this.CURRENCY))
                {
                    errors.AppendLine("CURRENCY değeri boş olamaz");
                }

                if (string.IsNullOrEmpty(this.BACK_REF))
                {
                    errors.AppendLine("BACK_REF değeri boş olamaz");
                }

               

                if (string.IsNullOrEmpty(this.LANGUAGE))
                {
                    errors.AppendLine("LANGUAGE değeri boş olamaz");
                }

                if (string.IsNullOrEmpty(this.MERCHANT))
                {
                    errors.AppendLine("MERCHANT değeri boş olamaz");
                }

                if (string.IsNullOrEmpty(this.PAY_METHOD))
                {
                    errors.AppendLine("PAY_METHOD değeri boş olamaz");
                }

                if (string.IsNullOrEmpty(this.PRICES_CURRENCY))
                {
                    errors.AppendLine("PRICES_CURRENCY değeri boş olamaz");
                }

              

                return errors;
            }
        }
        public class PayUCreditCard
        {
            public PayUCreditCard()
            {
            }

          
            public string MASKED_CC_NUMBER { get; set; }

            public string Get_MASKED_CC_NUMBER()
            {
                if (!string.IsNullOrEmpty(this.Get_CC_NUMBER()) && this.Get_CC_NUMBER().Length > 10)
                {
                    return this.Get_CC_NUMBER().Substring(0, 10);
                }
                return string.Empty;
            }

            public string CC_NUMBER { get; set; }

            public string Get_CC_NUMBER()
            {
                return this.CC_NUMBER;
            }

            public PayUCreditCard Set_CC_NUMBER(string value)
            {
                this.CC_NUMBER = (value);
                return this;
            }

            public string EXP_MONTH { get; set; }

            public string Get_EXP_MONTH()
            {
                return this.EXP_MONTH;
            }

            public PayUCreditCard Set_EXP_MONTH(string value)
            {
                this.EXP_MONTH = (value);
                return this;
            }

            public string EXP_YEAR { get; set; }

            public string Get_EXP_YEAR()
            {
                return this.EXP_YEAR;
            }

            public PayUCreditCard Set_EXP_YEAR(string value)
            {
                this.EXP_YEAR = (value);
                return this;
            }

            public string CC_TYPE { get; set; }

            public string Get_CC_TYPE()
            {
                return this.CC_TYPE;
            }

            public PayUCreditCard Set_CC_TYPE(string value)
            {
                this.CC_TYPE = (value);
                return this;
            }

            public string CC_CVV { get; set; }

            public string Get_CC_CVV()
            {
                return this.CC_CVV;
            }

            public PayUCreditCard Set_CC_CVV(string value)
            {
                this.CC_CVV = (value);
                return this;
            }

            public string CC_OWNER { get; set; }

            public string Get_CC_OWNER()
            {
                return this.CC_OWNER;
            }

            public PayUCreditCard Set_CC_OWNER(string value)
            {
                this.CC_OWNER = (value);
                return this;
            }

            public string SELECTED_INSTALLMENTS_NUMBER { get; set; }

            public string Get_SELECTED_INSTALLMENTS_NUMBER()
            {
                if (this.SELECTED_INSTALLMENTS_NUMBER == "0" || this.SELECTED_INSTALLMENTS_NUMBER == "1")
                {
                    return string.Empty;
                }
                else
                {
                    return this.SELECTED_INSTALLMENTS_NUMBER;
                }
            }

            public PayUCreditCard Set_SELECTED_INSTALLMENTS_NUMBER(string value)
            {
                this.SELECTED_INSTALLMENTS_NUMBER = (value);
                return this;
            }

            public string CARD_PROGRAM_NAME { get; set; }

            public string Get_CARD_PROGRAM_NAME()
            {
                return this.CARD_PROGRAM_NAME;
            }

            public PayUCreditCard Set_CARD_PROGRAM_NAME(string value)
            {
                this.CARD_PROGRAM_NAME = (value);
                return this;
            }

            public string CC_TOKEN { get; set; }

        }

        public class PayUCustomer
        {
            public PayUCustomer()
            {

            }

        
            public string CLIENT_IP { get; set; }

            public string CLIENT_TIME { get; set; }

            /// <summary>Client firstname</summary>
            public string BILL_FNAME { get; set; }

            /// <summary>Client lastname</summary>
            public string BILL_LNAME { get; set; }

            /// <summary>Company name for billing</summary>
            public string BILL_COMPANY { get; set; }

            /// <summary>E-mail address</summary>
            public string BILL_EMAIL { get; set; }

            /// <summary>Phone number</summary>
            public string BILL_PHONE { get; set; }

            /// <summary>fax Number</summary>
            public string BILL_FAX { get; set; }


            /// <summary>Customer/Company physical address</summary>
            public string BILL_ADDRESS { get; set; }

            /// <summary>Customer/Company address (second line)</summary>
            public string BILL_ADDRESS2 { get; set; }

            /// <summary>Customer/Company zip code</summary>
            public string BILL_ZIPCODE { get; set; }

   
            public string BILL_CITY { get; set; }

   
            public string BILL_STATE { get; set; }

        
            public string BILL_COUNTRYCODE { get; set; }

        }
        public class PayUDelivery
        {
            public string DELIVERY_FNAME { get; set; }
            public string DELIVERY_LNAME { get; set; }
            public string DELIVERY_EMAIL { get; set; }
            public string DELIVERY_PHONE { get; set; }
            public string DELIVERY_COMPANY { get; set; }
            public string DELIVERY_ADDRESS { get; set; }
            public string DELIVERY_ADDRESS2 { get; set; }
            public string DELIVERY_ZIPCODE { get; set; }
            public string DELIVERY_CITY { get; set; }
            public string DELIVERY_STATE { get; set; }
            public string DELIVERY_COUNTRYCODE { get; set; }
        }

        private static string ConvertDecimalToString(decimal value)
        {
            return String.Format("{0:0.00}", (value).ToString(CultureInfo.CreateSpecificCulture("en-GB")));
        }

        public static ApiPaymentNon3DResponse Non3DExecute(ApiPaymentRequest request, Options options)
        {
            var hashString= CreateHashString(request, options.SecretKey);
            return HttpCaller.Create().PostData<ApiPaymentNon3DResponse>(options.Url, hashString);
        }
        public static ApiPayment3DResponse ThreeDSecurePayment(ApiPaymentRequest request, Options options)
        {
            var hashString = CreateHashString(request, options.SecretKey);
            return HttpCaller.Create().PostData<ApiPayment3DResponse>(options.Url, hashString);
        }

        public static BkmPaymentResponse BkmPayment(ApiPaymentRequest request, Options options)
        {
            var hashString = CreateHashString(request, options.SecretKey);
            return HttpCaller.Create().PostData<BkmPaymentResponse>(options.Url, hashString);
        }


        public static string CreateHashString(ApiPaymentRequest apiPaymentRequest, string secretKey)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("MERCHANT", apiPaymentRequest.Config.MERCHANT);
            dictionary.Add("ORDER_REF", apiPaymentRequest.Order.Get_ORDER_REF());
            dictionary.Add("ORDER_DATE", apiPaymentRequest.Order.Get_ORDER_DATE());
            dictionary.Add("ORDER_SHIPPING", apiPaymentRequest.Order.Get_ORDER_SHIPPING());
            dictionary.Add("LU_ENABLE_TOKEN", apiPaymentRequest.Config.LU_ENABLE_TOKEN);
            int i=  0;
            foreach (var item in apiPaymentRequest.Order.OrderItems)
            {
                dictionary.Add("ORDER_PNAME[" + i.ToString() + "]", item.Get_ORDER_PNAME());
                dictionary.Add("ORDER_PCODE[" + i.ToString() + "]", item.Get_ORDER_PCODE());
                dictionary.Add("ORDER_PINFO[" + i.ToString() + "]", item.Get_ORDER_PINFO());
                dictionary.Add("ORDER_PRICE[" + i.ToString() + "]",  item.Get_ORDER_PRICE());
                dictionary.Add("ORDER_QTY[" + i.ToString() + "]", item.Get_ORDER_QTY());
                dictionary.Add("ORDER_VAT[" + i.ToString() + "]", item.Get_ORDER_VAT());
                dictionary.Add("ORDER_PRICE_TYPE[" + i.ToString() + "]", item.Get_ORDER_PRICE_TYPE());
                i++;
            }
            dictionary.Add("PRICES_CURRENCY" , apiPaymentRequest.Config.PRICES_CURRENCY);
            dictionary.Add("PAY_METHOD" , apiPaymentRequest.Config.PAY_METHOD);
            dictionary.Add("CC_NUMBER" , apiPaymentRequest.CreditCard.Get_CC_NUMBER());
            dictionary.Add("EXP_MONTH" , apiPaymentRequest.CreditCard.Get_EXP_MONTH());
            dictionary.Add("EXP_YEAR" , apiPaymentRequest.CreditCard.Get_EXP_YEAR());
            dictionary.Add("CC_CVV" , apiPaymentRequest.CreditCard.Get_CC_CVV());
            dictionary.Add("CC_OWNER" , apiPaymentRequest.CreditCard.Get_CC_OWNER());
            dictionary.Add("CC_TOKEN", apiPaymentRequest.CreditCard.CC_TOKEN);
            dictionary.Add("BACK_REF" , apiPaymentRequest.Config.BACK_REF);
            dictionary.Add("CLIENT_IP" , apiPaymentRequest.Customer.CLIENT_IP);
            dictionary.Add("CLIENT_TIME" , apiPaymentRequest.Customer.CLIENT_TIME);
            if (!string.IsNullOrEmpty(apiPaymentRequest.CreditCard.Get_SELECTED_INSTALLMENTS_NUMBER()))
            {
                dictionary.Add("SELECTED_INSTALLMENTS_NUMBER" , apiPaymentRequest.CreditCard.Get_SELECTED_INSTALLMENTS_NUMBER());
            }
            dictionary.Add("BILL_LNAME" , apiPaymentRequest.Customer.BILL_LNAME);
            dictionary.Add("BILL_FNAME" , apiPaymentRequest.Customer.BILL_FNAME);
            dictionary.Add("BILL_EMAIL" , apiPaymentRequest.Customer.BILL_EMAIL);
            dictionary.Add("BILL_PHONE" , apiPaymentRequest.Customer.BILL_PHONE);
            dictionary.Add("BILL_FAX" , apiPaymentRequest.Customer.BILL_FAX);
            dictionary.Add("BILL_ADDRESS" , apiPaymentRequest.Customer.BILL_ADDRESS);
            dictionary.Add("BILL_ADDRESS2" , apiPaymentRequest.Customer.BILL_ADDRESS2);
            dictionary.Add("BILL_ZIPCODE" , apiPaymentRequest.Customer.BILL_ZIPCODE);
            dictionary.Add("BILL_CITY" , apiPaymentRequest.Customer.BILL_CITY);
            dictionary.Add("BILL_COUNTRYCODE" , apiPaymentRequest.Customer.BILL_COUNTRYCODE);
            dictionary.Add("BILL_STATE" , apiPaymentRequest.Customer.BILL_STATE);
            dictionary.Add("DELIVERY_FNAME" , apiPaymentRequest.Delivery.DELIVERY_FNAME);
            dictionary.Add("DELIVERY_LNAME" , apiPaymentRequest.Delivery.DELIVERY_LNAME);
            dictionary.Add("DELIVERY_EMAIL" , apiPaymentRequest.Delivery.DELIVERY_EMAIL);
            dictionary.Add("DELIVERY_PHONE" , apiPaymentRequest.Delivery.DELIVERY_PHONE);
            dictionary.Add("DELIVERY_COMPANY" , apiPaymentRequest.Delivery.DELIVERY_COMPANY);
            dictionary.Add("DELIVERY_ADDRESS" , apiPaymentRequest.Delivery.DELIVERY_ADDRESS);
            dictionary.Add("DELIVERY_ADDRESS2" , apiPaymentRequest.Delivery.DELIVERY_ADDRESS2);
            dictionary.Add("DELIVERY_ZIPCODE" , apiPaymentRequest.Delivery.DELIVERY_ZIPCODE);
            dictionary.Add("DELIVERY_CITY" , apiPaymentRequest.Delivery.DELIVERY_CITY);
            dictionary.Add("DELIVERY_STATE" , apiPaymentRequest.Delivery.DELIVERY_STATE);
            dictionary.Add("DELIVERY_COUNTRYCODE" , apiPaymentRequest.Delivery.DELIVERY_COUNTRYCODE);
            var ordered = dictionary.OrderBy(x => x.Key);
            string hash = string.Empty;
            foreach (var item in ordered)
            {
                hash += "&"+ item.Key + "=" + item.Value;
            }
            hash= hash.Substring(1);
            hash += "&ORDER_HASH=" + CreateMD5Hash(secretKey,dictionary);
          
            return hash;
        }
        protected static string CreateMD5Hash(string secretKey, Dictionary<string, string> dictionary)
        {
            string HASH_CONTENT = string.Empty;
            string HASHED_CONTENT = string.Empty;
            var dictionaryOrder = dictionary.OrderBy(x => x.Key);
            foreach (var item in dictionaryOrder)
            {
                HASH_CONTENT += Helper.GetLengthAsByte(item.Value) + item.Value;
            }
            HASHED_CONTENT = Helper.CreateHash(HASH_CONTENT, secretKey);
            return HASHED_CONTENT;
        }
    }
   
}