using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PermacallWebApp.Models.ReturnModels;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;

namespace PermacallWebApp.Models.ViewModels
{
    public class UserManagementModel
    {
        public List<User> UserList { get; set; }
    }
}