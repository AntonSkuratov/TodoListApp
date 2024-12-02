using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Core.Entities;
using TodoListApp.Core.Infrastructure.Security;
using TodoListApp.Core.Interfaces;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Core.Records.Response;

namespace TodoListApp.Storage.Repositories
{
	public class UserRepository : IUserRepository
	{
		/// <summary>
		/// Добавляет роль пользователю.
		/// На вход принимает id пользователя и id роли.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="roleId"></param>
		public void AddRole(int userId, int roleId)
		{
			using (var context = new DatabaseContext())
			{
				var user = context.Users.First(x => x.Id == userId);
				var role = context.Roles.First(x => x.Id == roleId);
				user.Roles.Add(role);
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Изменяет статус блокировки пользователя на противоположный.
		/// На вход принимает id пользователя.
		/// </summary>
		/// <param name="id"></param>
		public void ChangeBlockingStatus(int id)
		{
			using (var context = new DatabaseContext())
			{
				var user = context.Users.First(x => x.Id == id);
				user.IsBlocked = !user.IsBlocked;
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Создаёт нового пользователя.
		/// Пользователь создаётся на основех данных CreateUserRequest.
		/// </summary>
		/// <param name="userRequest"></param>
		public void Create(CreateUserRequest userRequest)
		{
			using (var context = new DatabaseContext())
			{
				var user = new User
				{
					Username = userRequest.Username,
					Lastname = userRequest.Lastname,
					Firstname = userRequest.Firstname,
					RefreshToken = TokenHandler.GetRefreshToken()
				};
				context.Users.Add(user);

				var domainLogin = new DomainLogin
				{
					Login = userRequest.Login,
					User = user
				};
				context.DomainLogins.Add(domainLogin);

				var salt = PasswordHandler.GenerateSalt();
				var localLogin = new LocalLogin
				{
					Login = userRequest.Login,
					Salt = salt,
					PasswordHash = PasswordHandler.GenerateHash(userRequest.Password, salt),
					User = user
				};
				context.LocalLogins.Add(localLogin);

				context.SaveChanges();
			}
		}


		/// <summary>
		/// Удаляет роль у пользователя.
		/// На вход принимает id пользователя и id роли.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="roleId"></param>
		public void DeleteRole(int userId, int roleId)
		{
			using (var context = new DatabaseContext())
			{
				var user = context.Users
				.Include(u => u.Roles)
				.First(x => x.Id == userId);
				var role = context.Roles.First(x => x.Id == roleId);
				user.Roles.Remove(role);
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Удаляет пользователя по его id.
		/// </summary>
		/// <param name="id"></param>
		public void Delete(int id)
		{
			using (var context = new DatabaseContext())
			{
				var user = context.Users.First(x => x.Id == id);
				context.Users.Remove(user);
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Возвращает всех пользователей в виде списка объектов GetAllUsersResponse.
		/// </summary>
		/// <returns></returns>
		public List<GetAllUsersResponse> GetAll()
		{
			using (var context = new DatabaseContext())
			{
				var users = context.Users
				.AsNoTracking()
				.Include(u => u.DomainLogin)
				.Include(u => u.LocalLogin)
				.ToList();
				var allUsers = new List<GetAllUsersResponse>();
				foreach (var user in users)
				{
					var userResponse = new GetAllUsersResponse(
						user.Id,
						user.Username!,
						user.Lastname!,
						user.Firstname!,
						user.IsBlocked,
						user.DomainLogin!.Login!,
						user.LocalLogin!.Login!);
					allUsers.Add(userResponse);
				}
				return allUsers.ToList();
			}
		}


		/// <summary>
		/// Возвращает пользователя в виде объекта GetUserResponse.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public GetUserResponse GetUser(int id)
		{
			using (var context = new DatabaseContext())
			{
				var user = context.Users
				.AsNoTracking()
				.Include(u => u.DomainLogin)
				.Include(u => u.LocalLogin)
				.Include(u => u.Roles)
				.ThenInclude(r => r.Permissions)
				.First(u => u.Id == id);

				var roles = user.Roles.Select(r => r.Name).ToList();

				var permissions = user.Roles
					.SelectMany(r => r.Permissions)
					.Select(p => p.Name)
					.Distinct()
					.ToList();

				var userInfo = new GetUserResponse(user.Username!, user.Lastname!, user.Firstname!,
					user.LocalLogin!.Login!, user.DomainLogin!.Login!,
					user.IsBlocked, roles!, permissions!);

				return userInfo;
			}
		}


		/// <summary>
		/// Возвращает пользователя по его логину и паролю
		/// </summary>
		/// <param name="login"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public User GetUserByLogin(string login, string password)
		{
			using (var context = new DatabaseContext())
			{
				var userLogin = context.Users
				.AsNoTracking()
				.Include(u => u.LocalLogin)
				.FirstOrDefault(u => u.LocalLogin!.Login == login);
				if (userLogin== null)
					return null!;

				var salt = userLogin.LocalLogin!.Salt;
				var user = context.Users
					.AsNoTracking()
					.Include(u => u.LocalLogin)
					.Include(u => u.Roles)
					.FirstOrDefault(u => u.LocalLogin!.Login == login
						&& PasswordHandler.GenerateHash(password, salt!) == u.LocalLogin.PasswordHash);
				return user!;
			}
		}


		/// <summary>
		/// Возвращает пользователя по его refresh-токену.
		/// </summary>
		/// <param name="refreshToken"></param>
		/// <returns></returns>
		public User GetUserByRefreshToken(string refreshToken)
		{
			using (var context = new DatabaseContext())
			{
				return context.Users
				.AsNoTracking()
				.FirstOrDefault(u => u.RefreshToken == refreshToken)!;
			}
		}


		/// <summary>
		/// Возвращает пользователей в виде списка объектов GetAllUsersResponse, 
		/// у которых имя пользователя или имя или фамилия содержат подстроку searchString.
		/// </summary>
		/// <param name="searchString"></param>
		/// <returns></returns>
		public List<GetAllUsersResponse> Search(string searchString)
		{
			using (var context = new DatabaseContext())
			{
				var users = context.Users
				.AsNoTracking()
				.Include(u => u.DomainLogin)
				.Include(u => u.LocalLogin)
				.Where(u => u.Username!.ToLower().Contains(searchString.ToLower())
					|| u.Lastname!.ToLower().Contains(searchString.ToLower())
					|| u.Firstname!.ToLower().Contains(searchString.ToLower()))
				.ToList();
				var allUsers = new List<GetAllUsersResponse>();
				foreach (var user in users)
				{
					var userResponse = new GetAllUsersResponse(
						user.Id,
						user.Username!,
						user.Lastname!,
						user.Firstname!,
						user.IsBlocked,
						user.DomainLogin!.Login!,
						user.LocalLogin!.Login!);
					allUsers.Add(userResponse);
				}
				return allUsers.ToList();
			}
		}


		/// <summary>
		/// Обновляет данные пользователя по его id.
		/// 1-ый параметр принимает новое имя пользователя.
		/// 2-ой параметр принимает новое имя.
		/// 3-ий параметр принимает новую фамилию.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="username"></param>
		/// <param name="lastname"></param>
		/// <param name="firstname"></param>
		public void Update(int id, string username, string lastname, string firstname)
		{
			using (var context = new DatabaseContext())
			{
				var user = context.Users.First(u => u.Id == id);
				user.Username = username;
				user.Lastname = lastname;
				user.Firstname = firstname;
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Возвращает пользователя по его логину.
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		public User GetUserByLogin(string login)
		{
			using (var context = new DatabaseContext())
			{
				return context.Users
				.AsNoTracking()
				.Include(u => u.LocalLogin)
				.First(u => u.LocalLogin!.Login == login);
			}
		}
	}
}
