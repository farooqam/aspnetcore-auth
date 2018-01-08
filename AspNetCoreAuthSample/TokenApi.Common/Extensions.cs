namespace TokenApi.Common
{
    public static class Extensions
    {
        public static ApiError WithData(this ApiError apiError, object data)
        {
            apiError.Data = data;
            return apiError;
        }
    }
}