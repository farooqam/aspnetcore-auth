using System.Collections.Generic;

namespace TokenApi.Common
{
    public class ApiError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static ApiError CreateTokenAuthFailure => new ApiError
        {
            Code = 3000,
            Message = "Could not create token. The username, password, and audience could not be verified.",
        };

        public static ApiError MissingUsername = new ApiError
        {
            Code = 3000,
            Message = "Specify a valid username."
        };

        public static ApiError MissingPassword = new ApiError
        {
            Code = 3001,
            Message = "Specify a valid password."
        };

        public static ApiError MissingAudience = new ApiError
        {
            Code = 3002,
            Message = "Specify an audience."
        };
        

    }
    
}