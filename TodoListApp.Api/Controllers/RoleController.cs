using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Storage.Repositories;

namespace TodoListApp.Api.Controllers
{
	[Authorize(Roles = "Admin,Watcher")]
	public class RoleController : Controller
	{
		//Эндпоинт для создания новой роли
		[HttpPost]
		[Authorize(Policy = "PostPermission")]
		[Route("/Role/Create")]
		public IActionResult Create(string name, string description)
		{
			var role = new CreateRoleRequest(name, description);
			new RoleRepository().CreateRole(role);

			return Ok();
		}

		//Эндпоинт для получения всех ролей
		[HttpGet]
		[Authorize(Policy = "GetPermission")]
		[Route("/Role/GetAll")]
		public IActionResult GetAll()
		{
			return Ok(new RoleRepository().GetAllRoles());
		}

		//Эндпоинт для обновления роли по id
		[HttpPut]
		[Authorize(Policy = "PutPermission")]
		[Route("/Role/Update")]
		public IActionResult Update(int id, string name, string description)
		{
			new RoleRepository().UpdateRole(id, name, description);
			return Ok();
		}

		//Эндпоинт для получения ролей с помощью пагинации и строки поиска
		[HttpGet]
		[Authorize(Policy = "GetPermission")]
		[Route("/Role/Search")]
		public IActionResult Search(string searchString, int pageNumber, int pageSize)
		{
			return Ok(new RoleRepository().GetRoles(searchString, pageNumber, pageSize));
		}

		//Эндпоинт для удаления роли по id
		[HttpDelete]
		[Authorize(Policy = "DeletePermission")]
		[Route("/Role/Delete")]
		public IActionResult Delete(int id)
		{
			new RoleRepository().DeleteRole(id);
			return Ok();
		}

		//Эндпоинт для копирования роли по id
		[HttpPost]
		[Authorize(Policy = "PostPermission")]
		[Route("/Role/Copy")]
		public IActionResult Copy(int id)
		{
			new RoleRepository().CopyRole(id);
			return Ok();
		}

		//Эндпоинт для добавления разрешения в роль
		//1-ый параметр принимает id разрешения
		//2-ой параметр принимает id роли
		[HttpPut]
		[Authorize(Policy = "PutPermission")]
		[Route("/Role/AddPermission")]
		public IActionResult AddPermission(int permissionId, int roleId)
		{
			new RoleRepository().AddPermission(permissionId, roleId);
			return Ok();
		}

		//Эндпоинт для удаления разрешения из роли
		//1-ый параметр принимает id разрешения
		//2-ой параметр принимает id роли
		[HttpPut]
		[Authorize(Policy = "PutPermission")]
		[Route("/Role/DeletePermission")]
		public IActionResult DeletePermission(int permissionId, int roleId)
		{
			new RoleRepository().DeletePermission(permissionId, roleId);
			return Ok();
		}
	}
}
