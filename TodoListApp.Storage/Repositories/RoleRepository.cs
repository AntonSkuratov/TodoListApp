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

		public void DeleteRole(int id)
		{
			using (var context = new DatabaseContext())
			{
				var role = context.Roles.First(x => x.Id == id);
				context.Roles.Remove(role);
				context.SaveChanges();
			}
		}

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
