using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Records.Response
{
	public record GetUserInfoResponse(
		string Username,
		string Lastname,
		string Firstname,
		string LocalLogin,
		string DomainLogin,
		bool IsBlocked,
		List<string> Roles,
		List<string> Permissions);
}
