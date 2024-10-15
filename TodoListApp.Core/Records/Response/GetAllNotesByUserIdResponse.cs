using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Records.Response
{
	public record GetAllNotesByUserIdResponse(
		int Id,
		string Title,
		string Description);
}
