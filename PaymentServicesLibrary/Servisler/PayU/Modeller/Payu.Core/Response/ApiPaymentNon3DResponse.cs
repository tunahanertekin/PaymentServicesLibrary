using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Response
{
    [XmlRoot("EPAYMENT")]
    public  class ApiPaymentNon3DResponse
    {
        public string REFNO { get; set; }
        public string ALIAS { get; set; }
        public string STATUS { get; set; }
        public string RETURN_CODE { get; set; }
        public string RETURN_MESSAGE { get; set; }
        public string DATE { get; set; }
        public string AMOUNT { get; set; }
        public string CURRENCY { get; set; }
        public string INSTALLMENTS_NO { get; set; }
        public string CARD_PROGRAM_NAME { get; set; }
        public string ORDER_REF { get; set; }
        public string AUTH_CODE { get; set; }
        public string RRN { get; set; }
        public string ERRORMESSAGE { get; set; }
        public string PROCRETURNCODE { get; set; }
        public string BANK_MERCHANT_ID { get; set; }
        public string PAN { get; set; }
        public string EXPYEAR { get; set; }
        public string EXPMONTH { get; set; }
        public string CLIENTID { get; set; }
        public string HOSTREFNUM { get; set; }
        public string OID { get; set; }
        public string RESPONSE { get; set; }
        public string TERMINAL_BANK { get; set; }
        public string MDSTATUS { get; set; }
        public string MDERRORMSG { get; set; }
        public string TXSTATUS { get; set; }
        public string XID { get; set; }
        public string ECI { get; set; }
        public string CAVV { get; set; }
        public string TRANSID { get; set; }
        public string HASH { get; set; }
        public string TOKEN_HASH { get; set; }
    }
}
