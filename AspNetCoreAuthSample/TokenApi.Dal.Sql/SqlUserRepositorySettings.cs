using System;

namespace TokenApi.Dal.Sql
{
    public class SqlUserRepositorySettings
    {
        public string ConnectionString { get; set; }
        public TimeSpan QueryTimeout { get; set; }
    }
}