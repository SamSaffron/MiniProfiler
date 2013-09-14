using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;

namespace Sample.WebForms_NET35
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var mp = MiniProfiler.Current;

            using (mp.Step("Page_Load"))
            {
                System.Threading.Thread.Sleep(40);
            }

            /* Warning: Try to login from login.aspx in order to get ASPNETDB created. */

            SelectTables();
        }

        private void SelectTables()
        {
            var profiler = MiniProfiler.Current;
            var bareFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            var providerFactory = new ProfiledDbProviderFactory(profiler, bareFactory);

            var connStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

            var bareConnection = new SqlConnection(connStr);

            var profiledConnection = new ProfiledDbConnection(bareConnection, profiler);

            var profiledCommand = providerFactory.CreateCommand();
            profiledCommand.Connection = profiledConnection;
            profiledCommand.CommandText = "SELECT * FROM sys.tables";

            using (profiler.Step("Open Connection"))
            {
                profiledConnection.Open();
            }

            using (profiler.Step("ExecuteNonQuery"))
            {
                profiledCommand.ExecuteNonQuery();
            }
        }
    }
}
