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
		public void AddRole(int userId, int roleId)
		{
			using (var context = new TodoListContext())
			{
				var user = context.Users.First(x => x.Id == userId);
				var role = context.Roles.First(x => x.Id == roleId);
				user.Roles.Add(role);
				context.SaveChanges();
			}		
		}

		public void ChangeBlockingStatus(int id)
		{
			using (var context = new TodoListContext())
			{
				var user = context.Users.First(x => x.Id == id);
				user.IsBlocked = !user.IsBlocked;
				context.SaveChanges();
			}			
		}

		public void CreateUser(CreateUserRequest userRequest)
		{
			using (var context = new TodoListContext())
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

		public void DeleteRole(int userId, int roleId)
		{
			using (var context = new TodoListContext())
			{
				var user = context.Users
				.Include(u => u.Roles)
				.First(x => x.Id == userId);
				var role = context.Roles.First(x => x.Id == roleId);
				user.Roles.Remove(role);
				context.SaveChanges();
			}		
		}

		public void DeleteUser(int id)
		{
			using (var context = new TodoListContext())
			{
				var user = context.Users.First(x => x.Id == id);
				context.Users.Remove(user);
				context.SaveChanges();
			}
		}

		public List<GetAllUsersResponse> GetAllUsers()
		{
			using (var context = new TodoListContext())
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

		public GetUserInfoResponse GetUser(int id)
		{
			using (var context = new TodoListContext())
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

				var userInfo = new GetUserInfoResponse(user.Username!, user.Lastname!, user.Firstname!,
					user.LocalLogin!.Login!, user.DomainLogin!.Login!,
					user.IsBlocked, roles!, permissions!);

				return userInfo;
			}
		}

		public User GetUserByLogin(string login, string password)
		{
			using (var context = new TodoListContext())
			{
				var salt = context.Users
				.AsNoTracking()
				.Include(u => u.LocalLogin)
				.First(u => u.LocalLogin!.Login == login)!.LocalLogin!.Salt;

				var user = context.Users
					.AsNoTracking()
					.Include(u => u.LocalLogin)
					.Include(u => u.Roles)
					.FirstOrDefault(u => u.LocalLogin!.Login == login
						&& PasswordHandler.GenerateHash(password, salt!) == u.LocalLogin.PasswordHash);
				return user!;
			}		
		}

		public User GetUserByRefreshToken(string refreshToken)
		{
			using (var context = new TodoListContext())
			{
				return context.Users
				.AsNoTracking()
				.FirstOrDefault(u => u.RefreshToken == refreshToken)!;
			}			
		}

		public List<GetAllUsersResponse> SearchUsers(string searchString)
		{
			using (var context = new TodoListContext())
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

		public void UpdateUser(int id, string username, string lastname, string firstname)
		{
			using (var context = new TodoListContext())
			{
				var user = context.Users.First(u => u.Id == id);
				user.Username = username;
				user.Lastname = lastname;
				user.Firstname = firstname;
				context.SaveChanges();
			}			
		}

		public User GetUserByLogin(string login)
		{
			using (var context = new TodoListContext())
			{
				return context.Users
				.AsNoTracking()
				.Include(u => u.LocalLogin)
				.First(u => u.LocalLogin!.Login == login);
			}
		}
	}
}
