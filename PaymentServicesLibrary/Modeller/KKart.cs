using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentServicesLibrary.Modeller
{
    public class KKart
    {
        public String KartNo = "";
        public String SonKulAy = "";
        public String SonKulYil = "";
        public String cvc2 = "";

        public KKart(string kartNo, string sonKulAy, string sonKulYil, string cvc2)
        {
            KartNo = kartNo;
            SonKulAy = sonKulAy;
            SonKulYil = sonKulYil;
            this.cvc2 = cvc2;
        }
    }
}
