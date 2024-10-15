using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Core.Entities;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Core.Records.Response;

namespace TodoListApp.Core.Interfaces
{
    public interface IUserRepository
	{
		void CreateUser(CreateUserRequest userRequest);
		void UpdateUser(int id, string username,
			string lastname,
			string firstname);
		void DeleteUser(int id);
		List<GetAllUsersResponse> GetAllUsers();
		List<GetAllUsersResponse> SearchUsers(string searchString);
		GetUserInfoResponse GetUser(int id);
		void ChangeBlockingStatus(int id);
		void AddRole(int userId, int roleId);		
		void DeleteRole(int userId, int roleId);
		User GetUserByLogin(string login, string password);
		User GetUserByLogin(string login);
		User GetUserByRefreshToken(string refreshToken);
	}
}
