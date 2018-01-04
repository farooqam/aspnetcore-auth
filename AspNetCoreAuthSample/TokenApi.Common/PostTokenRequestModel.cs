namespace TokenApi.Common
{
    public class PostTokenRequestModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}