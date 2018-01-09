using System;
using System.Collections.Generic;

namespace TokenApi.Dal.Common
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<RegisteredApplicationDto> RegisteredApplications { get; set; }
    }
}