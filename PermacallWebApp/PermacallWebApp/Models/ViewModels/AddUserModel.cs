using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermacallWebApp.Models.ViewModels
{
	public class AddUserModel
	{
	    public string Password { get; set; }
	    public string ChannelName { get; set; }

	    public string TSName { get; set; }
	    public bool ChannelEmpty { get; set; }
	    public bool UserAdded { get; set; }
	    public bool StartedAdding { get; set; }

	    public string ErrorMessage { get; set; }
	}
}