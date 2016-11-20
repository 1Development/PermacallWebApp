namespace PermacallTools.Controllers
{
    public class Database
    {
        public static string ConnectionString
        {
            get
            {
                return LoginData.DatabaseString;
            }
        }
    }
}