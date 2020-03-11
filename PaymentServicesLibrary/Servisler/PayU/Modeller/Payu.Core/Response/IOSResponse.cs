using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Response
{
    [XmlRoot("Order")]
    public class IOSResponse
    {
        public string ORDER_DATE { get; set; }
        public string REFNO { get; set; }
        public string REFNOEXT { get; set; }
        public string PAYMETHOD { get; set; }
        public string ORDER_STATUS { get; set; }
        public string ORDER_HASH { get; set; }
    }
}
