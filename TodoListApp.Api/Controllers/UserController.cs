using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TodoListApp.Api.Infrastructure;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Storage.Repositories;

namespace TodoListApp.Api.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		//Эндпоинт для создания нового пользователя
		[AllowAnonymous]
		[HttpPost]
		[Route("/User/Create")]
		public IActionResult Create(string username, string lastname, string firstname,
			string login, string password)
		{
			var userRequest = new CreateUserRequest(username, lastname, firstname, login, password);
			new UserRepository().Create(userRequest);

			var user = new UserRepository().GetUserByLogin(login);
			var defautRole = new RoleRepository().GetRoleByName(RoleRepository.DefaultRoleName);
			new UserRepository().AddRole(user.Id, defautRole.Id);
			return Ok();
		}

		//Эндпоинт для обновления текущего пользователя
		//Id текущего пользователя берётся из токена
		[Authorize(Policy = "ModifyAccountPermission")]
		[HttpPut]
		[Route("/User/Update")]
		public IActionResult Modify(string username, string lastname, string firstname)
		{
			new UserRepository().Update(Convert.ToInt32(User.FindFirst("Id")!.Value),
				username, lastname, firstname);

			return Ok();
		}

		//Эндпоинт для получения всех пользователей
		[HttpGet]
		[Authorize(Policy = "GetPermission")]
		[Route("/User/GetAll")]
		public IActionResult GetAll()
		{
			return Ok(new UserRepository().GetAll());
		}

		//Эндпоинт для удаления пользователя по его id
		[Authorize(Policy = "DeletePermission")]
		[HttpDelete]
		[Route("/User/Delete")]
		public IActionResult Delete(int userId)
		{
			new UserRepository().Delete(userId);
			return Ok();
		}

		//Эндпоинт для получения пользователей по строке поиска
		[Authorize(Policy = "GetPermission")]
		[HttpGet]
		[Route("/User/Search")]
		public IActionResult Search(string searchString)
		{
			return Ok(new UserRepository().Search(searchString));
		}

		//Эндпоинт для получения текущего пользователя
		//Id текущего пользователя берётся из токена
		[Authorize(Policy = "GetCurrentUserData")]
		[HttpGet]
		[Route("/User/Get")]
		public IActionResult GetUser()
		{
			return Ok(new UserRepository().GetUser(Convert.ToInt32(User.FindFirst("Id")!.Value)));
		}

		//Эндпоинт пользователя по id		
		[Authorize(Policy = "GetPermission")]
		[HttpGet]
		[Route("/User/GetId")]
		public IActionResult GetUserById(int id)
		{
			return Ok(new UserRepository().GetUser(id));
		}

		//Эндпоинт для добавления пользователя в роль
		//1-ый параметр принимает id пользователя
		//2-ой параметр принимает id роли
		[Authorize(Policy = "PutPermission")]
		[HttpPut]
		[Route("/User/AddRole")]
		public IActionResult AddRole(int userId, int roleId)
		{
			new UserRepository().AddRole(userId, roleId);
			return Ok();
		}

		//Эндпоинт для удаления пользователя из роли
		//1-ый параметр принимает id пользователя
		//2-ой параметр принимает id роли
		[Authorize(Policy = "PutPermission")]
		[HttpPut]
		[Route("/User/DeleteRole")]
		public IActionResult DeleteRole(int userId, int roleId)
		{
			new UserRepository().DeleteRole(userId, roleId);
			return Ok();
		}

		//Эндпоинт для изменеия статуса блокировки пользователя по его id
		[Authorize(Policy = "PutPermission")]
		[HttpPut]
		[Route("/User/UpdateBlockStatus")]
		public IActionResult UpdateBlockStatus(int userId)
		{
			new UserRepository().ChangeBlockingStatus(userId);
			return Ok();
		}
	}
}
