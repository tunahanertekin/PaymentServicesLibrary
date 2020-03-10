using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentServicesLibrary.Servisler.Moka.Modeller
{
    public class PaymentDealerRequest
    {
        public string CardHolderFullName { get; set; }
        public string CardNumber { get; set; }
        public string ExpMonth { get; set; }
        public string ExpYear { get; set; }
        public string CvcNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int InstallmentNumber { get; set; }
        public string VirtualPosOrderId { get; set; }
        public int VoidRefundReason { get; set; }
        public string ClientIP { get; set; }
        public string RedirectUrl { get; set; }
        public int UtilityCompanyBillId { get; set; }
        public int DealerStaffId { get; set; }
        public string OtherTrxCode { get; set; }
        public int IsPreAuth { get; set; }

    }
}