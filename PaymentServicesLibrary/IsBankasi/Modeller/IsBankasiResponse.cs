using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentServicesLibrary.IsBankasi.Modeller
{
    public class IsbankResponse
    {
        public VposResponse VposResponse;

        public IsbankResponse(VposResponse vposResponse)
        {
            VposResponse = vposResponse;
        }
    }
}
