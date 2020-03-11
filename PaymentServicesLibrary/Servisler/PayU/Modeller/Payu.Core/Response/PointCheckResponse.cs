using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Response
{
    [XmlRoot("EPAYMENT")]
    public class PointCheckResponse
    {
        public string STATUS { get; set; }
        public string MESSAGE { get; set; }
        public string POINTS { get; set; }
        public string AMOUNT { get; set; }
        public string CURRENCY { get; set; }
        public string BANK { get; set; }
        public string CARD_PROGRAM_NAME { get; set; }
        public string DATE { get; set; }
        public string HASH { get; set; }
    }
}
