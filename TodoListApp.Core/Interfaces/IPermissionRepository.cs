using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Core.Entities;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Core.Records.Response;

namespace TodoListApp.Core.Interfaces
{
	//Интерфейс для реализации CRUD-методов над разрешениями 
	public interface IPermissionRepository
	{
		void CreatePermission(CreatePermissionRequest permissionRequest);
		void UpdatePermission(int id, string name, string description);
		void DeletePermission(int id);
		List<GetAllPermissionsResponse> GetAllPermissions();
	}
}
