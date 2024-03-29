﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PaymentServicesLibrary.Servisler.PayU.Modeller.Response
{
    [XmlRoot("EPAYMENT")]
    public class ApiPayment3DResponse
    {
        public string REFNO { get; set; }
        public string ALIAS { get; set; }
        public string STATUS { get; set; }
        public string RETURN_CODE { get; set; }
        public string RETURN_MESSAGE { get; set; }
        public string DATE { get; set; }
        public string URL_3DS { get; set; }
        public string HASH { get; set; }
    }
}
