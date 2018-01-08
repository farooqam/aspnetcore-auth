using System.Collections.Generic;
using System.Linq;

namespace TokenApi.Common
{
    public class ApiErrors
    {
        public int ErrorCount => Errors?.Count() ?? 0;
        public string Operation { get; set; }
        public IEnumerable<ApiError> Errors { get; set; }

        public static IEnumerable<ApiError> ErrorList = new List<ApiError>
        {
            ApiError.CreateTokenAuthFailure,
            ApiError.MissingPassword,
            ApiError.MissingAudience,
            ApiError.MissingUsername
        };
    }
}