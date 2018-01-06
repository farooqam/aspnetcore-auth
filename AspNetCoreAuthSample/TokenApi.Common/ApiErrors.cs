using System.Collections.Generic;
using System.Linq;

namespace TokenApi.Common
{
    public class ApiErrors
    {
        public int ErrorCount => Errors?.Count() ?? 0;
        public IEnumerable<ApiError> Errors { get; set; }

    }
}