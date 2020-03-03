using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentServicesLibrary.IsBankasi.Modeller
{
    public class Enrollment
    {
        public IPaySecure IPaySecure;

        public Enrollment(IPaySecure ıPaySecure)
        {
            IPaySecure = ıPaySecure;
        }
    }
}