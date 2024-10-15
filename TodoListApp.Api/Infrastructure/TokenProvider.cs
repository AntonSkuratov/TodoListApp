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
	public static class TokenProvider
	{
		public const string ISSUER = "TodoListAppServer";
		public const string AUDIENCE = "TodoListAppServerClient";
		const string KEY = "!over123secretkey!123987over456secret!key123987over789secretkey123987!";
		public static SymmetricSecurityKey GetSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
		}

		public static TokenModel GetTokens(User user)
		{
			var refreshToken = new RefreshToken(user.RefreshToken!);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Username!),
				new Claim("Id", user.Id.ToString()),							
			};
			foreach (var role in user.Roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Name!.ToString()));
			}

			var permissions = new UserRepository().GetUser(user.Id).Permissions;
			foreach(var permission in permissions)
			{
				claims.Add(new Claim("Permission", permission));
			}			

			var token = new JwtSecurityToken(
				issuer: ISSUER,
				audience: AUDIENCE,
				claims: claims,
				expires: DateTime.Now.AddSeconds(5),
				signingCredentials: new SigningCredentials(
					GetSecurityKey(), SecurityAlgorithms.HmacSha256));
			var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
			var tokenModel = new TokenModel(accessToken, refreshToken.Token);
			return tokenModel;
		}		
	}
}
