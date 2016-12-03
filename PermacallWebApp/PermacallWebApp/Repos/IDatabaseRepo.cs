using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermacallWebApp.Repos
{
    public interface IDatabaseRepo
    {
        bool CheckExist(string SQLquery, Dictionary<string, string> parameters);
        Dictionary<string, string> GetOneResultQuery(string SQLquery, Dictionary<string, string> parameters);
        List<Dictionary<string, string>> GetMultipleResultsQuery(string SQLquery, Dictionary<string, string> parameters);
        bool UpdateQuery(string SQLquery, Dictionary<string, string> parameters);
        bool DeleteQuery(string SQLquery, Dictionary<string, string> parameters);
        bool InsertQuery(string SQLquery, Dictionary<string, string> parameters);
    }
}
