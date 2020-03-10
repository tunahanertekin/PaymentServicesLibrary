using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentServicesLibrary.Servisler.Moka.Modeller
{
    /*
     *"DealerPaymentId": 6781196,
                "OtherTrxCode": "MK-9AF3360A-4BB7-4DEC-9001-85D0F0BA0D80",
                "CardHolderFullName": "Mustafa YILDIZ",
                "CardNumberFirstSix": "552096",
                "CardNumberLastFour": "6363",
                "PaymentDate": "2020-02-07T15:38:57.52",
                "Amount": 5.00,
                "RefAmount": 0.00,
                "CurrencyCode": "TL",
                "InstallmentNumber": 3,
                "DealerCommissionAmount": 0.22,
                "IsThreeD": true,
                "Description": "",
                "PaymentStatus": 2,
                "TrxStatus": 2,
                "Software": "MokaPosWeb" 
     */

    public class Payment
    {
        public int DealerPaymentId { get; set; }
        public String OtherTrxCode { get; set; }
        public String CardHolderFullName { get; set; }
        public String CardNumberFirstSix { get; set; }
        public String CardNumberLastFour { get; set; }
        public String PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public decimal RefAmount { get; set; }
        public String CurrencyCode { get; set; }
        public int InstallmentNumber { get; set; }
        public decimal DealerCommissionAmount { get; set; }
        public bool IsThreeD { get; set; }
        public String Description { get; set; }
        public int PaymentStatus { get; set; }
        public int TrxStatus { get; set; }
        public String Software { get; set; }

    }
}