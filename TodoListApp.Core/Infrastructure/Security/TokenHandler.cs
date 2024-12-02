using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Infrastructure.Security
{

	public static class TokenHandler
	{
		//Метод для генерации refresh-токена
		public static string GetRefreshToken() => 
			Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	}
}
