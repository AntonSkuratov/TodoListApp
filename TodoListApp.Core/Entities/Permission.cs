using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Entities
{
	public class Permission
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }


		//Список ролей которые меют данное разрешение
		public List<Role> Roles { get; set; } = new();
	}
}
