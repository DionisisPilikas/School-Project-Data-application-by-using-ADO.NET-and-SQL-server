using System.Configuration;

namespace SERVISES_NS
{
    public class Connection_String
    {
        protected static string connectionstring =
            ConfigurationManager.ConnectionStrings["BOOTCAMP_DB_CONNECTION"].ConnectionString;
    }
}