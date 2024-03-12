using System.Configuration;

namespace LpakBL.Controller
{
    public static class ConnectionStringFactory
    {
        private static string GetNameConnection => "ConnectionStringDBLpakTesting";
        public static string GetConnectionString()
        {
            var connectionStringName = GetNameConnection;
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
    }
}