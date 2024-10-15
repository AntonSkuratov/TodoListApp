using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Storage.Repositories;

namespace TodoListApp.Api.Controllers
{
	[Authorize(Roles = "Admin,Watcher")]
	public class PermissionController : Controller
	{
		[HttpPost]
		[Authorize(Policy = "PostPermission")]
		[Route("/Permission/CreatePermission")]
		public IActionResult CreatePermission(string name, string description)
		{
			var permission = new CreatePermissionRequest(name, description);
			new PermissionRepository().CreatePermission(permission);

			return Ok();
		}

		[HttpDelete]
		[Authorize(Policy = "DeletePermission")]
		[Route("/Permission/DeletePermission")]
		public IActionResult DeletePermission(int id)
		{
			new PermissionRepository().DeletePermission(id);
			return Ok();
		}

		[HttpGet]
		[Authorize(Policy = "GetPermission")]
		[Route("/Permission/GetAllPermissions")]
		public IActionResult GetAllPermissions()
		{
			return Ok(new PermissionRepository().GetAllPermissions());
		}

		[HttpGet]
		[Authorize(Policy = "GetPermission")]
		[Route("/Permission/GetPermissions")]
		public IActionResult GetPermissions(int pageNumber, int pageSize)
		{
			return Ok(new PermissionRepository().GetPermissions(pageNumber, pageSize));
		}

		[HttpPut]
		[Authorize(Policy = "PutPermission")]
		[Route("/Permission/UpdatePermission")]
		public IActionResult UpdatePermission(int id, string name, string description)
		{
			new PermissionRepository().UpdatePermission(id, name, description);
			return Ok();
		}
	}
}
