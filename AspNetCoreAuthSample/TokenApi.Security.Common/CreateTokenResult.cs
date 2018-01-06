namespace TokenApi.Security.Common
{
    public class CreateTokenResult
    {
        public string Token { get; set; }
        public string Issuer { get; set; }
        public long ValidFrom { get; set; }
        public long ValidTo { get; set; }
    }
}