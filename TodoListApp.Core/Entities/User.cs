using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string? Username { get; set; }
		public string? Lastname { get; set; }
		public string? Firstname { get; set; }

		//refresh-токн пользователя(не имеет срока жизни)
		public string? RefreshToken { get; set; }

		public bool IsBlocked { get; set; }
		public DomainLogin? DomainLogin { get; set; }
		public LocalLogin? LocalLogin { get; set; }


		//Список ролей которые меет данный пользователь
		public List<Role> Roles { get; set; } = new();
	}
}
