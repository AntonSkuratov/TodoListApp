using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Records.Response
{
	public record class GetAllUsersResponse(
		int id,
		string Username,
		string Lastname,
		string Firstname,
		bool IsBlocked,
		string DomainLogin,
		string LocalLogin);
}
