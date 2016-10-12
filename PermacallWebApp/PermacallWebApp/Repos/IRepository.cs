using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PermacallWebApp.Models.ReturnModels;

namespace PermacallWebApp.Repos
{
    public interface IRepository
    {
        Tuple<bool, string> GetSalt(string username);
        bool CheckAvailable(string username);
        Tuple<bool, string> ValidateCredentials(string username, string password);
        bool SetSessionKey(string username, string sessionKey);
        User GetUser(string sessionKey);
        bool InsertNewAccount(string username, string password, string salt);
    }
}
