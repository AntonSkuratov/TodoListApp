using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Storage.Repositories;

namespace TodoListApp.Api.Controllers
{
	[Authorize(Roles = "Admin,Watcher")]
	public class RoleController : Controller
	{
		[HttpPost]
		[Authorize(Policy = "PostPermission")]
		[Route("/Role/Create")]
		public IActionResult Create(string name, string description)
		{
			var role = new CreateRoleRequest(name, description);
			new RoleRepository().CreateRole(role);

			return Ok();
		}

		[HttpGet]
		[Authorize(Policy = "GetPermission")]
		[Route("/Role/GetAll")]
		public IActionResult GetAll()
		{
			return Ok(new RoleRepository().GetAllRoles());
		}

		[HttpPut]
		[Authorize(Policy = "PutPermission")]
		[Route("/Role/Update")]
		public IActionResult Update(int id, string name, string description)
		{
			new RoleRepository().UpdateRole(id, name, description);
			return Ok();
		}

		[HttpGet]
		[Authorize(Policy = "GetPermission")]
		[Route("/Role/Search")]
		public IActionResult Search(string searchString, int pageNumber, int pageSize)
		{
			return Ok(new RoleRepository().GetRoles(searchString, pageNumber, pageSize));
		}

		[HttpDelete]
		[Authorize(Policy = "DeletePermission")]
		[Route("/Role/Delete")]
		public IActionResult Delete(int id)
		{
			new RoleRepository().DeleteRole(id);
			return Ok();
		}

		[HttpPost]
		[Authorize(Policy = "PostPermission")]
		[Route("/Role/Copy")]
		public IActionResult Copy(int id)
		{
			new RoleRepository().CopyRole(id);
			return Ok();
		}

		[HttpPut]
		[Authorize(Policy = "PutPermission")]
		[Route("/Role/AddPermission")]
		public IActionResult AddPermission(int permissionId, int roleId)
		{
			new RoleRepository().AddPermission(permissionId, roleId);
			return Ok();
		}

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
