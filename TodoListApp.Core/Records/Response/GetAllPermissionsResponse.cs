using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Records.Response
{
	public record GetAllPermissionsResponse(
		int Id,
		string Name,
		string Description,
		List<string> Roles);
}
