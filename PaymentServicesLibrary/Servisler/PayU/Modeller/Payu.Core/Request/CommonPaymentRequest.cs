using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Request
{
    /// <summary>
    /// Ortak ödeme sayfası için kullanılan sınıfı temsil etmektedir.
    /// ilgili parametreler MD5 ile şifrelenerek post edilmektedir.
    /// Sorgu sonucunda oluşan değerler son kullanıcıya gösterilmektedir.
    /// </summary>
    public class CommonPaymentRequest
    {
        public PayUOrder Order { get; set; }
        public PayUConfig Config { get; set; }
      
        public PayUCustomer Customer { get; set; }
        public PayUDelivery Delivery { get; set; }


        public CommonPaymentRequest()
        {
            Order = new PayUOrder();
            Config = new PayUConfig();
            Customer = new PayUCustomer();
            Delivery = new PayUDelivery();
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

            public string MERCHANT { get; set; }

            public string PRICES_CURRENCY { get; set; }

        

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

            public string BACK_REF { get; set; }

            public string TESTORDER { get; set; }

           
            public string LANGUAGE { get; set; }

            public string LU_ENABLE_TOKEN { get; set; }

        }
        public class PayUOrder
        {
            public List<PayUOrderItem> OrderItems { get; set; }

            public PayUOrder()
            {
                OrderItems = new List<PayUOrderItem>();
            }

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


        public class PayUCustomer
        {
            public PayUCustomer()
            {

            }
            public string BILL_FNAME { get; set; }

            public string BILL_LNAME { get; set; }

            public string BILL_EMAIL { get; set; }
            public string BILL_PHONE { get; set; }

            public string BILL_COUNTRYCODE { get; set; }
            public string BILL_FAX { get; set; }
           
            public string BILL_ADDRESS { get; set; }
            public string BILL_ADDRESS2 { get; set; }
            public string BILL_ZIPCODE { get; set; }
            public string BILL_CITY { get; set; }
            public string BILL_STATE { get; set; }
            public string DESTINATION_CITY { get; set; }
            public string DESTINATION_STATE { get; set; }
            public string DESTINATION_COUNTRY { get; set; }

        }
        private static string ConvertDecimalToString(decimal value)
        {
            return String.Format("{0:0.00}", (value).ToString(CultureInfo.CreateSpecificCulture("en-GB")));
        }

        public static string Execute(CommonPaymentRequest request,Options options)
        {
            var hashString = CreateMD5Hash(request, options.SecretKey);
            //return HttpCaller.PostDataReturnString(options.Url, hashString);


            StringBuilder builder = new StringBuilder();
            builder.Append("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">");
            builder.Append("<html>");
            builder.Append("<body>");
            builder.Append("<form //action=\"" + options.Url + "\" method=\"post\" id=\"common_form\" >");
            builder.Append("<input type=\"hidden\" name=\"MERCHANT\" value=\"" + request.Config.MERCHANT + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"ORDER_REF\" value=\"" + request.Order.ORDER_REF + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BACK_REF\" value=\"" + request.Config.BACK_REF + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"ORDER_DATE\" value=\"" + request.Order.ORDER_DATE + "\"/>");

            foreach (var item in request.Order.OrderItems)
            {
                builder.Append("<input type=\"hidden\" name=\"ORDER_PNAME[]\" value=\"" + item.ORDER_PNAME + "\"/>");
            }
            foreach (var item in request.Order.OrderItems)
            {
                builder.Append("<input type=\"hidden\" name=\"ORDER_PCODE[]\" value=\"" + item.ORDER_PCODE + "\"/>");
            }
            foreach (var item in request.Order.OrderItems)
            {
                builder.Append("<input type=\"hidden\" name=\"ORDER_PINFO[]\" value=\"" + item.ORDER_PINFO + "\"/>");
            }
            foreach (var item in request.Order.OrderItems)
            {
                builder.Append("<input type=\"hidden\" name=\"ORDER_PRICE[]\" value=\"" + item.ORDER_PRICE + "\"/>");
            }
            foreach (var item in request.Order.OrderItems)
            {
                builder.Append("<input type=\"hidden\" name=\"ORDER_PRICE_TYPE[]\" value=\"" + item.ORDER_PRICE_TYPE + "\"/>");
            }
            foreach (var item in request.Order.OrderItems)
            {
                builder.Append("<input type=\"hidden\" name=\"ORDER_QTY[]\" value=\"" + item.ORDER_QTY + "\"/>"); 
            }
            foreach (var item in request.Order.OrderItems)
            {
                builder.Append("<input type=\"hidden\" name=\"ORDER_VAT[]\" value=\"" + item.ORDER_VAT + "\"/>");
            }

            builder.Append("<input type=\"hidden\" name=\"ORDER_SHIPPING\" value=\"" + request.Order.ORDER_SHIPPING + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_FNAME\" value=\"" + request.Customer.BILL_FNAME + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_LNAME\" value=\"" + request.Customer.BILL_LNAME + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_EMAIL\" value=\"" + request.Customer.BILL_EMAIL + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_PHONE\" value=\"" + request.Customer.BILL_PHONE + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_FAX\" value=\"" + request.Customer.BILL_FAX + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_ADDRESS\" value=\"" + request.Customer.BILL_ADDRESS + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_ADDRESS2\" value=\"" + request.Customer.BILL_ADDRESS2 + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_ZIPCODE\" value=\"" + request.Customer.BILL_ZIPCODE + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_CITY\" value=\"" + request.Customer.BILL_CITY + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_COUNTRYCODE\" value=\"" + request.Customer.BILL_COUNTRYCODE + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"BILL_STATE\" value=\"" + request.Customer.BILL_STATE + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_FNAME\" value=\"" + request.Delivery.DELIVERY_FNAME + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_LNAME\" value=\"" + request.Delivery.DELIVERY_LNAME + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_EMAIL\" value=\"" + request.Delivery.DELIVERY_EMAIL + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_PHONE\" value=\"" + request.Delivery.DELIVERY_PHONE + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_COMPANY\" value=\"" + request.Delivery.DELIVERY_COMPANY + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_ADDRESS\" value=\"" + request.Delivery.DELIVERY_ADDRESS + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_ADDRESS2\" value=\"" + request.Delivery.DELIVERY_ADDRESS2 + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_ZIPCODE\" value=\"" + request.Delivery.DELIVERY_ZIPCODE + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_CITY\" value=\"" + request.Delivery.DELIVERY_CITY + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_STATE\" value=\"" + request.Delivery.DELIVERY_STATE + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DELIVERY_COUNTRYCODE\" value=\"" + request.Delivery.DELIVERY_COUNTRYCODE + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"PRICES_CURRENCY\" value=\"" + request.Config.PRICES_CURRENCY + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DISCOUNT\" value=\"" + request.Order.DISCOUNT + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DESTINATION_CITY\" value=\"" + request.Customer.DESTINATION_CITY + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DESTINATION_STATE\" value=\"" + request.Customer.DESTINATION_STATE + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"DESTINATION_COUNTRY\" value=\"" + request.Customer.DESTINATION_COUNTRY + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"PAY_METHOD\" value=\"" + request.Config.PAY_METHOD + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"TESTORDER\" value=\"" + request.Config.TESTORDER + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"LANGUAGE\" value=\"" + request.Config.LANGUAGE + "\"/>");
            builder.Append("<input type=\"hidden\" name=\"ORDER_HASH\" value=\"" + hashString + "\"/>");
            builder.Append("<input type=\"submit\" value=\"Öde\" style=\"display:none;\"/>");
            builder.Append("<noscript>");
            builder.Append("<br/>");
            builder.Append("<br/>");
            builder.Append("</noscript>");
            builder.Append("</form>");
            builder.Append("</body>");
            builder.Append("<script>document.getElementById(\"common_form\").submit();</script>");
            builder.Append("</html>");
            return builder.ToString();


        }

      
        protected static string CreateMD5Hash( CommonPaymentRequest request,string secretKey)
        {
            string HASH_CONTENT = string.Empty;
            string HASHED_CONTENT = string.Empty;  
                HASH_CONTENT += Helper.GetLengthAsByte(request.Config.MERCHANT) + request.Config.MERCHANT;
                HASH_CONTENT += Helper.GetLengthAsByte(request.Order.ORDER_REF) + request.Order.ORDER_REF;
                HASH_CONTENT += Helper.GetLengthAsByte(request.Order.ORDER_DATE) + request.Order.ORDER_DATE;

            foreach (var item in request.Order.OrderItems)
            {
                HASH_CONTENT+=  Helper.GetLengthAsByte(item.ORDER_PNAME) + item.ORDER_PNAME;     
            }
            foreach (var item in request.Order.OrderItems)
            {
                HASH_CONTENT += Helper.GetLengthAsByte(item.ORDER_PCODE) + item.ORDER_PCODE;
            }
            foreach (var item in request.Order.OrderItems)
            {
                HASH_CONTENT += Helper.GetLengthAsByte(item.ORDER_PINFO) + item.ORDER_PINFO;
            }
            foreach (var item in request.Order.OrderItems)
            {
                HASH_CONTENT += Helper.GetLengthAsByte(item.ORDER_PRICE) + item.ORDER_PRICE;
            }
            
            foreach (var item in request.Order.OrderItems)
            {
                HASH_CONTENT += Helper.GetLengthAsByte(item.ORDER_QTY) + item.ORDER_QTY;
            }
            foreach (var item in request.Order.OrderItems)
            {
                HASH_CONTENT += Helper.GetLengthAsByte(item.ORDER_VAT) + item.ORDER_VAT;
            }

            HASH_CONTENT += Helper.GetLengthAsByte(request.Order.ORDER_SHIPPING) + request.Order.ORDER_SHIPPING;
            HASH_CONTENT += Helper.GetLengthAsByte(request.Config.PRICES_CURRENCY) + request.Config.PRICES_CURRENCY;
            HASH_CONTENT += Helper.GetLengthAsByte(request.Order.DISCOUNT) + request.Order.DISCOUNT;
            HASH_CONTENT += Helper.GetLengthAsByte(request.Customer.DESTINATION_CITY) + request.Customer.DESTINATION_CITY;
            HASH_CONTENT += Helper.GetLengthAsByte(request.Customer.DESTINATION_STATE) + request.Customer.DESTINATION_STATE;
            HASH_CONTENT += Helper.GetLengthAsByte(request.Customer.DESTINATION_COUNTRY) + request.Customer.DESTINATION_COUNTRY;
            HASH_CONTENT += Helper.GetLengthAsByte(request.Config.PAY_METHOD) + request.Config.PAY_METHOD;
            foreach (var item in request.Order.OrderItems)
            {
                HASH_CONTENT += Helper.GetLengthAsByte(item.ORDER_PRICE_TYPE) + item.ORDER_PRICE_TYPE;
            }
            HASH_CONTENT += Helper.GetLengthAsByte(request.Config.TESTORDER) + request.Config.TESTORDER;
            HASHED_CONTENT = Helper.CreateHash(HASH_CONTENT, secretKey);
            return HASHED_CONTENT;
        }

    }
}
