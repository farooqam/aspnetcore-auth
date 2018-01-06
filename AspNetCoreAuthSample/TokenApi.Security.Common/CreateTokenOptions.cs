namespace TokenApi.Security.Common
{
    public class CreateTokenOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Audience { get; set; }
    }
}