using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentServicesLibrary.Servisler.Moka.Modeller
{
    public class PaymentDetail
    {
        public int DealerPaymentId { get; set; }
        public string OtherTrxCode { get; set; }
        public string CardHolderFullName { get; set; }
        public string CardNumberFirstSix { get; set; }
        public string CardNumberLastFour { get; set; }
        public string PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public decimal RefAmount { get; set; }
        public string CurrencyCode { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal DealerCommissionAmount { get; set; }
        public bool IsThreeD { get; set; }
        public string Description { get; set; }
        public int PaymentStatus { get; set; }
        public int TrxStatus { get; set; }
        public string Software { get; set; }

    }
}