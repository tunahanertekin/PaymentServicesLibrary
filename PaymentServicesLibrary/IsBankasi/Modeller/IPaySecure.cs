using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentServicesLibrary.IsBankasi.Modeller
{
    public class IPaySecure
    {
        public VERes VERes;
        public String VerifyEnrollmentRequestId;
        public String MessageErrorCode;

        public IPaySecure(VERes vERes, string verifyEnrollmentRequestId, string messageErrorCode)
        {
            VERes = vERes;
            VerifyEnrollmentRequestId = verifyEnrollmentRequestId;
            MessageErrorCode = messageErrorCode;
        }
    }
}