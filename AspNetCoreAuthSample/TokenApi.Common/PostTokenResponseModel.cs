﻿namespace TokenApi.Common
{
    public class PostTokenResponseModel
    {
        public string Token { get; set; }
        public string Issuer { get; set; }
        public long ValidFrom { get; set; }
        public long ValidTo { get; set; }
    }
}