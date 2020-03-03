using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.IsBankasi.Modeller
{
    public class VposResponse
    {
        public String MerchantId;
        public String TransactionType;
        public String TransactionId;
        public String ResultCode;
        public String ResultDetail;
        public String AuthCode;
        public String HostDate;
        public String Rrn;
        public String TerminalNo;
        public String CurrencyAmount;
        public String CurrencyCode;
        public String ECI;
        public String ThreeDSecureType;
        public String TransactionDeviceSource;
        public String BatchNo;
    }
}
