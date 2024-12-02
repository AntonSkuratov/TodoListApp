using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Infrastructure.Security
{
	public static class PasswordHandler
	{
		//Метод для генерации случайной соли
		public static string GenerateSalt()
		{
			byte[] saltBytes = new byte[16];
			var randomNumberGenerator = RandomNumberGenerator.Create();
			randomNumberGenerator.GetBytes(saltBytes);
			return Convert.ToBase64String(saltBytes);
		}

		//Метод для генерации хэша пароля из строки пароля и соли
		public static string GenerateHash(string password, string salt)
		{
			byte[] passwordSalt = Encoding.UTF8.GetBytes(password + salt);
			var sha256 = SHA256.Create();
			return Convert.ToBase64String(sha256.ComputeHash(passwordSalt));
		}
	}
}
