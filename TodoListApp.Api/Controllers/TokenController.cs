using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Api.Infrastructure;
using TodoListApp.Storage.Repositories;

namespace TodoListApp.Api.Controllers
{
	public class TokenController : Controller
	{
		//Эндпоинт для получения пары из access- и refresh- токенов
		//На вход принимает логин и пароль пользователя
		[HttpGet]
		[AllowAnonymous]
		[Route("/Token/Get")]
		public IActionResult Get(string login, string password)
		{
			var user = new UserRepository().GetUserByLogin(login, password);

			if (user == null)
				return BadRequest("Неверный логин или пароль");

			if (user.IsBlocked)
				return BadRequest("Данный пользователь заблокирован");

			return Ok(TokenProvider.GetTokens(user));
		}

		//Эндпоинт для получения нового access токена
		//Используется когда истёк срок жизни крайнего токена
		[AllowAnonymous]
		[HttpGet]
		[Route("/Token/GetNew")]
		public IActionResult GetNew(string refreshToken)
		{
			var user = new UserRepository().GetUserByRefreshToken(refreshToken);
			if (user is null)
				return BadRequest("Неверный refresh-токен");

			return Ok(TokenProvider.GetTokens(user));
		}
	}
}
