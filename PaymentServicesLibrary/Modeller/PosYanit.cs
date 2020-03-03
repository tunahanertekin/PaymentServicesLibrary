using System;
using System.Collections.Generic;
using System.Text;


namespace PaymentServicesLibrary.Modeller
{
    public class PosYanit
    {
        public DateTime Tarih = DateTime.Now;
        public String Yanit = "";
        public String RefNo = "";
        public String IslemNo = "";
        public int Onay = 0;
        public String Hata = "";
        public String detay = "";
        public double Odeme = 0;
        public int PosID = 0;

        public PosYanit(DateTime tarih, string yanit, string refNo, string ıslemNo, int onay, string hata, string detay, double odeme, int posID)
        {
            Tarih = tarih;
            Yanit = yanit;
            RefNo = refNo;
            IslemNo = ıslemNo;
            Onay = onay;
            Hata = hata;
            this.detay = detay;
            Odeme = odeme;
            PosID = posID;
        }
        
        public PosYanit()
        {
            
        }
    }
}
