using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using TodoListApp.Api.Models;
using TodoListApp.Core.Entities;
using TodoListApp.Storage.Repositories;

namespace TodoListApp.Api.Infrastructure
{
	//Static класс для получния access- и refresh- токенов
	public static class TokenProvider
	{
		//Константы для:	
		//времени жизни access-токена
		public const int LIFE_TIME = 5;
		//издателя токена
		public const string ISSUER = "TodoListAppServer";
		//потребителя токена
		public const string AUDIENCE = "TodoListAppServerClient";
		//секретного ключа
		const string KEY = "!over123secretkey!123987over456secret!key123987over789secretkey123987!";


		//Метод для получния SymmetricSecurityKey из строки ключа(const KEY)		
		public static SymmetricSecurityKey GetSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
		}



		//Метод для получния TokenModel(access- и refresh- токенов) по пользователю
		public static TokenModel GetTokens(User user)
		{
			//Создаётся объект RefreshToken для хранения refresh-токена
			var refreshToken = new RefreshToken(user.RefreshToken!);

			//Создаём клэймы(полезные данные для токена): Id и имя пользователя
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Username!),
				new Claim("Id", user.Id.ToString()),							
			};

			//Цикл проходит по всем ролям пользователя 
			//и создаёт клэйм для каждой роли
			foreach (var role in user.Roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Name!.ToString()));
			}

			//Цикл проходит по всем разрешениям пользователя 
			//и создаёт клэйм для каждого разрешения
			var permissions = new UserRepository().GetUser(user.Id).Permissions;
			foreach(var permission in permissions)
			{
				claims.Add(new Claim("Permission", permission));
			}

			//Создание JwtSecurityToken для генерации access-токена
			//Параметры токена берутся из констант
			var token = new JwtSecurityToken(
				issuer: ISSUER,
				audience: AUDIENCE,
				claims: claims,
				expires: DateTime.Now.AddMinutes(LIFE_TIME),
				signingCredentials: new SigningCredentials(
					GetSecurityKey(), SecurityAlgorithms.HmacSha256));
			var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
			var tokenModel = new TokenModel(accessToken, refreshToken.Token);

			//Возврат объекта TokenModel, который
			//содержит пару из access- и refresh- токенов
			return tokenModel;
		}		
	}
}
