using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Entities
{
	public class Role
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		//Список пользователей которые меют данную роль
		public List<User> Users { get; set; } = new();


		//Список разрешений которые меют данную роль
		public List<Permission> Permissions { get; set; } = new();
	}
}
