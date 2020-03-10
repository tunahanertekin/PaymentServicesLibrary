using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentServicesLibrary.Servisler.Moka.Modeller
{
    public class DealerPaymentServiceDirectPaymentResultData
    {
        public bool IsSuccessful { get; set; }
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
        public int ListItemCount { get; set; }
        public TrxDetail[] PaymentTrxDetailList { get; set; }
        public  Payment[] PaymentList { get; set; }

    }
}