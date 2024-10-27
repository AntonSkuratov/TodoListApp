using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using TodoListApp.Api.Infrastructure;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Storage.Repositories;

namespace TodoListApp.Api.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		[AllowAnonymous]
		[HttpPost]
		[Route("/Account/Registration")]
		public IActionResult Registration(string username, string lastname, string firstname,
			string login, string password)
		{
			var userRequest = new CreateUserRequest(username, lastname, firstname, login, password);
			new UserRepository().CreateUser(userRequest);

			var user = new UserRepository().GetUserByLogin(login);
			var defautRole = new RoleRepository().GetRoleByName(RoleRepository.DefaultRoleName);
			new UserRepository().AddRole(user.Id, defautRole.Id);
			return Ok();
		}

		[Authorize(Policy = "ModifyAccountPermission")]
		[HttpPut]
		[Route("/Account/Modify")]
		public IActionResult Modify(string username, string lastname, string firstname)
		{
			new UserRepository().UpdateUser(Convert.ToInt32(User.FindFirst("Id")!.Value),
				username, lastname, firstname);

			return Ok();
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("/Account/Authentication")]
		public IActionResult Authentication(string login, string password)
		{
			var user = new UserRepository().GetUserByLogin(login, password);

			if (user == null)
				return BadRequest("Неверный логин или пароль");

			if (user.IsBlocked)
				return BadRequest("Данный пользователь заблокирован");

			return Ok(TokenProvider.GetTokens(user));
		}

		[Authorize(Policy = "GetPermission")]
		[HttpGet]
		[Route("/Account/GetNewTokens")]
		public IActionResult GetNewTokens(string refreshToken)
		{
			var user = new UserRepository().GetUserByRefreshToken(refreshToken);
			if (user is null)
				return BadRequest("Неверный refresh-токен");

			return Ok(TokenProvider.GetTokens(user));
		}
	}
}
