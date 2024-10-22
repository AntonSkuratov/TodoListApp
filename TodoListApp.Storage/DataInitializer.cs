using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Core.Entities;

namespace TodoListApp.Storage
{
	public static class DataInitializer
	{
		public static List<Role> GetRoles()
		{
			return new List<Role>()
			{
				new Role { Id = 1, Name = "Admin", Description = "Администратор" },
				new Role { Id = 2, Name = "Watcher", Description = "Наблюдатель" },
				new Role { Id = 3, Name = "User", Description = "Пользователь" }
			};
		}

		public static List<Permission> GetPermissions()
		{
			return new List<Permission>
			{
				new Permission { Id = 1, Name = "ModifyAccount", Description = "Редактирование профиля" },
				new Permission { Id = 2, Name = "CreateNewAccountNote", Description = "Создание заметок для текущего профиля" },
				new Permission { Id = 3, Name = "Get", Description = "Get" },
				new Permission { Id = 4, Name = "Post", Description = "Post" },
				new Permission { Id = 5, Name = "Put", Description = "Put" },
				new Permission { Id = 6, Name = "Delete", Description = "Delete" }
			};
		}
	}
}
