using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentServicesLibrary.Servisler.Moka.Modeller
{
    public class AllPaymentsRequest
    {
        public PaymentDealerAuthentication PaymentDealerAuthentication { get; set; }
        public PaymentListPaymentDealerRequest PaymentDealerRequest { get; set; }
    }
}