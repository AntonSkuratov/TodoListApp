using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Core.Entities;
using TodoListApp.Core.Interfaces;
using TodoListApp.Core.Records.Requests;
using TodoListApp.Core.Records.Response;

namespace TodoListApp.Storage.Repositories
{
	public class RoleRepository : IRoleRepository
	{
		public static string DefaultRoleName { get; } = "User";


		/// <summary>
		/// Добавляет разрешение в роль.
		/// На вход принимает id разрешения и id роли.
		/// </summary>
		/// <param name="permissionId"></param>
		/// <param name="roleId"></param>
		public void AddPermission(int permissionId, int roleId)
		{
			using (var context = new DatabaseContext())
			{
				var permission = context.Permissions.First(x => x.Id == permissionId);
				var role = context.Roles.First(x => x.Id == roleId);
				role.Permissions.Add(permission);
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Копирует роль по её id и на основе этой роли создаёт новую с такими же полями.		
		/// </summary>
		/// <param name="id"></param>
		public void CopyRole(int id)
		{
			using (var context = new DatabaseContext())
			{
				var role = context.Roles.First(r => r.Id == id);
				var newRole = new Role
				{
					Name = role.Name,
					Description = role.Description,
					Users = role.Users,
					Permissions = role.Permissions
				};
				context.Roles.Add(newRole);
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Создаёт новую роль.
		/// роль создаётся на основех данных CreateRoleRequest.
		/// </summary>
		/// <param name="roleRequest"></param>
		public void CreateRole(CreateRoleRequest roleRequest)
		{
			using (var context = new DatabaseContext())
			{
				var role = new Role
				{
					Name = roleRequest.Name,
					Description = roleRequest.Description
				};
				context.Roles.Add(role);
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Удаляет разрешение из роли.
		/// На вход принимает id разрешения и id роли.
		/// </summary>
		/// <param name="permissionId"></param>
		/// <param name="roleId"></param>
		public void DeletePermission(int permissionId, int roleId)
		{
			using (var context = new DatabaseContext())
			{
				var permission = context.Permissions.First(x => x.Id == permissionId);
				var role = context.Roles.Include(r => r.Permissions).First(x => x.Id == roleId);
				role.Permissions.Remove(permission);
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Удаляет роль по её id
		/// </summary>
		/// <param name="id"></param>
		public void DeleteRole(int id)
		{
			using (var context = new DatabaseContext())
			{
				var role = context.Roles.First(x => x.Id == id);
				context.Roles.Remove(role);
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Возвращает все роли в виде списка объектов GetAllRolesResponse.
		/// </summary>
		/// <returns></returns>
		public List<GetAllRolesResponse> GetAllRoles()
		{
			using (var context = new DatabaseContext())
			{
				var roles = context.Roles
				.AsNoTracking()
				.Include(r => r.Permissions)
				.ToList();

				var rolesResponse = new List<GetAllRolesResponse>();
				foreach (var role in roles)
				{
					var roleResponse = new GetAllRolesResponse
					(
						role.Id,
						role.Name!,
						role.Description!,
						role.Permissions.Select(p => p.Name).ToList()!
					);
					rolesResponse.Add(roleResponse);
				}
				return rolesResponse;
			}
		}


		/// <summary>
		/// Возвращает роли(с пагинацией) в виде списка объектов GetAllRolesResponse, 
		/// у которых название или описание содержат подстроку searchString.
		/// 2-ой параметр принимает номер страницы.
		/// 3-ий параметр принимает размер страницы.
		/// </summary>
		/// <param name="searchString"></param>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public List<GetAllRolesResponse> GetRoles(string searchString, int pageNumber, int pageSize)
		{
			using (var context = new DatabaseContext())
			{
				var roles = context.Roles
				.AsNoTracking()
				.Include(r => r.Permissions)
				.Where(r => r.Name!.Contains(searchString) || 
					r.Description!.Contains(searchString))
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();

				var rolesResponse = new List<GetAllRolesResponse>();
				foreach (var role in roles)
				{
					var roleResponse = new GetAllRolesResponse
					(
						role.Id,
						role.Name!,
						role.Description!,
						role.Permissions.Select(p => p.Name).ToList()!
					);
					rolesResponse.Add(roleResponse);
				}
				return rolesResponse;
			}
		}


		/// <summary>
		/// Обновляет данные роли по его id.
		/// 1-ый параметр принимает новое название.
		/// 2-ой параметр принимает новое описание.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		public void UpdateRole(int id, string name, string description)
		{
			using (var context = new DatabaseContext())
			{
				var role = context.Roles.First(x => x.Id == id);
				role.Name = name;
				role.Description = description;
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Возвразает роль по его имени в виде GetAllRolesResponse
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public GetAllRolesResponse GetRoleByName(string name)
		{
			using (var context = new DatabaseContext())
			{
				var role = context.Roles.First(r => r.Name == name);
				var roleResponse = new GetAllRolesResponse
				(
					role.Id,
					role.Name!,
					role.Description!,
					role.Permissions.Select(p => p.Name).ToList()!
				);
				return roleResponse;
			}
		}
	}
}
