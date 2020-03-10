using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentServicesLibrary.Servisler.Moka.Modeller
{
    public class DealerPaymentServicePaymentRequest
    {
        public PaymentDealerAuthentication PaymentDealerAuthentication { get; set; }
        public PaymentDealerRequest PaymentDealerRequest { get; set; }
    }
}