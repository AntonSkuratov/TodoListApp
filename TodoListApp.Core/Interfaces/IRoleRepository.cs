using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Core.Entities;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Core.Records.Response;

namespace TodoListApp.Core.Interfaces
{
	//Интерфейс для реализации CRUD-методов над ролями 
	public interface IRoleRepository
	{
		void CreateRole(CreateRoleRequest roleRequest);
		void UpdateRole(int id, string name, string description);
		void DeleteRole(int id);
		List<GetAllRolesResponse> GetAllRoles();
		List<GetAllRolesResponse> GetRoles(string searchString, int pageNumber, int pageSize);
	}
}
