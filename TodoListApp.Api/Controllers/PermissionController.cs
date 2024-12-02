using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Storage.Repositories;

namespace TodoListApp.Api.Controllers
{
	[Authorize(Roles = "Admin,Watcher")]
	public class PermissionController : Controller
	{
		//Эндпоинт для создания нового разрешения
		[HttpPost]
		[Authorize(Policy = "PostPermission")]
		[Route("/Permission/Create")]
		public IActionResult Create(string name, string description)
		{
			var permission = new CreatePermissionRequest(name, description);
			new PermissionRepository().CreatePermission(permission);

			return Ok();
		}

		//Эндпоинт для удаления разрешения по id
		[HttpDelete]
		[Authorize(Policy = "DeletePermission")]
		[Route("/Permission/Delete")]
		public IActionResult Delete(int id)
		{
			new PermissionRepository().DeletePermission(id);
			return Ok();
		}

		//Эндпоинт для получения всех разрешений
		[HttpGet]
		[Authorize(Policy = "GetPermission")]
		[Route("/Permission/GetAll")]
		public IActionResult GetAlls()
		{
			return Ok(new PermissionRepository().GetAllPermissions());
		}

		//Эндпоинт для получения разрешений с помощью пагинации и строки поиска
		[HttpGet]
		[Authorize(Policy = "GetPermission")]
		[Route("/Permission/Search")]
		public IActionResult GetPermissions(string searchString, int pageNumber, int pageSize)
		{
			return Ok(new PermissionRepository().GetPermissions(searchString, pageNumber, pageSize));
		}

		//Эндпоинт для обновления разрешения по id
		[HttpPut]
		[Authorize(Policy = "PutPermission")]
		[Route("/Permission/Update")]
		public IActionResult Update(int id, string name, string description)
		{
			new PermissionRepository().UpdatePermission(id, name, description);
			return Ok();
		}
	}
}
