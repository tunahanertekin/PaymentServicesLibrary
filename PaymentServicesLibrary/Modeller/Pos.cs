using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentServicesLibrary.Modeller
{
    public class Pos
    {
        public int PosID = 0;
        public String PosBilgi = "";
        public String PosDonus = "";

        public Pos(int posID, string posBilgi, string posDonus)
        {
            PosID = posID;
            PosBilgi = posBilgi;
            PosDonus = posDonus;
        }
    }
}
