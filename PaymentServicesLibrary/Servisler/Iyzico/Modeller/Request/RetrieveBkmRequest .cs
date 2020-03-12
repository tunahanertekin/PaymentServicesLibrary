﻿using System;

namespace Iyzipay.Request
{
    public class RetrieveBkmRequest : BaseRequest
    {
        public String Token { set; get; }

        public override String ToPKIRequestString()
        {
            return ToStringRequestBuilder.NewInstance()
                .AppendSuper(base.ToPKIRequestString())
                .Append("token", Token)
                .GetRequestString();
        }
    }
}
