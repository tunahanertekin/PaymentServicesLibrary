using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentServicesLibrary.IsBankasi.Modeller
{
    public class VERes
    {
        public String Version;
        public String Status;
        public String PAReq;
        public String ACSUrl;
        public String TermUrl;
        public String MD;
        public String Xid;
        public String ACTUALBRAND;

        public VERes(string version, string status, string pAReq, string aCSUrl, string termUrl, string mD, string xid, string aCTUALBRAND)
        {
            Version = version;
            Status = status;
            PAReq = pAReq;
            ACSUrl = aCSUrl;
            TermUrl = termUrl;
            MD = mD;
            Xid = xid;
            ACTUALBRAND = aCTUALBRAND;
        }
    }
}