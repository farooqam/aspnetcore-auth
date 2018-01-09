using System;

namespace TokenApi.Security.Sql
{
    public class SqlCredentialValidatorSettings
    {
        public string ConnectionString { get; set; }
        public TimeSpan QueryTimeout { get; set; }
    }
}