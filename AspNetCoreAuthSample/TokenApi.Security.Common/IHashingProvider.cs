namespace TokenApi.Security.Common
{
    public interface IHashingProvider
    {
        string Hash(string value, bool caseSensitive = false);
    }
}