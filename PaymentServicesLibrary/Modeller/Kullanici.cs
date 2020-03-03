using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentServicesLibrary.Modeller
{
    public class Kullanici
    {
        public String KullaniciEmail = "";
        public String KullaniciAdi = "";

        public Kullanici(string kullaniciEmail, string kullaniciAdi)
        {
            KullaniciEmail = kullaniciEmail;
            KullaniciAdi = kullaniciAdi;
        }
    }
}
