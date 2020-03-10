using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentServicesLibrary.Servisler.Moka.Modeller
{
    public class TrxDetail
    {
        public int DealerPaymentTrxId { get; set; }
        public int DealerPaymentId { get; set; }
        public string TrxCode { get; set; }
        public string TrxDate { get; set; }
        public decimal Amount { get; set; }
        public int TrxType { get; set; }
        public int TrxStatus { get; set; }
        public int PaymentReason { get; set; }
        public int VoidRefundReason { get; set; }
        public string VirtualPosOrderId { get; set; }
        public string ResultMessage { get; set; }

    }
}