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
		void Create(CreateUserRequest userRequest);
		void Update(int id, string username,
			string lastname,
			string firstname);
		void Delete(int id);
		List<GetAllUsersResponse> GetAll();
		List<GetAllUsersResponse> Search(string searchString);		
		void ChangeBlockingStatus(int id);
		void AddRole(int userId, int roleId);		
		void DeleteRole(int userId, int roleId);		
	}
}
