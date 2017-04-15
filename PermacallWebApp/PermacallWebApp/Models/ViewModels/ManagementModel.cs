using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TS3QueryLib.Core.Common.Responses;
using TS3QueryLib.Core.Server.Entities;

namespace PermacallWebApp.Models
{
	public class ManagementModel
	{
	    public ListResponse<ClientListEntry> Users { get; set; }

	    public User CurrentUser  { get; set; }
	    public string ToChangeName { get; set; }
	    public int ToChangeID { get; set; }

	    public ManagementModel()
	    {
	        Users = new ListResponse<ClientListEntry>();
	    }
	}
}