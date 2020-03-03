using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentServicesLibrary.Modeller
{
    public class PosIslem
    {
        public String Odeme = "";
        public String Taksit = "";
        public String EKBilgiler = "";
        public Double Komisyon = 0;

        public PosIslem(string odeme, string taksit, string eKBilgiler, double komisyon)
        {
            Odeme = odeme;
            Taksit = taksit;
            EKBilgiler = eKBilgiler;
            Komisyon = komisyon;
        }
    }
}
