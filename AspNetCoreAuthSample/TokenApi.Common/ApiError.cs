namespace TokenApi.Common
{
    public class ApiError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static ApiError CreateTokenAuthFailure(PostTokenRequestModel errorData)
        {
            return new ApiError
            {
                Code = 2000,
                Message = "Could not create token. The username, password, and audience could not be verified.",
                Data = errorData
            };
        }
    }
    
}