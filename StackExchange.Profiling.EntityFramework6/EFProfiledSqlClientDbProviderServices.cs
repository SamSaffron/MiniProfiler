﻿namespace StackExchange.Profiling.Data
{
    using System.Data.Entity.Core.Common;
    using System.Data.Entity.Core.Common.CommandTrees;
    using System.Data.Entity.SqlServer;
    using System.Data.SqlClient;

    /// <summary>
    /// Specific implementation of <c>EFProfiledDbProviderFactory&lt;SqlClientFactory&gt;</c> to enable profiling
    /// </summary>
    public class EFProfiledSqlClientDbProviderServices
        : EFProfiledDbProviderServices<SqlProviderServices>
    {
        /// <summary>
        /// Every provider factory must have an Instance public field
        /// </summary>
        public static new EFProfiledSqlClientDbProviderServices Instance = new EFProfiledSqlClientDbProviderServices();

        /// <summary>
        /// Prevents a default instance of the <see cref="EFProfiledSqlClientDbProviderServices"/> class from being created.
        /// </summary>
        private EFProfiledSqlClientDbProviderServices()
        {
        }
    }
}
