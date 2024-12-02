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
	public class PermissionRepository : IPermissionRepository
	{
		/// <summary>
		/// Создаёт новое разрешение.
		/// Разрешение создаётся на основех данных CreatePermissionRequest.
		/// </summary>
		/// <param name="permissionRequest"></param>
		public void CreatePermission(CreatePermissionRequest permissionRequest)
		{
			using (var context = new DatabaseContext())
			{
				var permission = new Permission
				{
					Name = permissionRequest.Name,
					Description = permissionRequest.Description,
				};
				context.Permissions.Add(permission);
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Удаляет разрешение по его id
		/// </summary>
		/// <param name="id"></param>
		public void DeletePermission(int id)
		{
			using (var context = new DatabaseContext())
			{
				var permission = context.Permissions.First(x => x.Id == id);
				context.Permissions.Remove(permission);
				context.SaveChanges();
			}
		}


		/// <summary>
		/// Возвращает все разрешения в виде GetAllPermissionsResponse
		/// </summary>
		/// <returns></returns>
		public List<GetAllPermissionsResponse> GetAllPermissions()
		{
			using (var context = new DatabaseContext())
			{
				var permissions = context.Permissions
				.AsNoTracking()
				.Include(p => p.Roles)
				.ToList();
				var permissionsResponse = new List<GetAllPermissionsResponse>();

				foreach (var permission in permissions)
				{
					var permissionResponse = new GetAllPermissionsResponse
					(
						permission.Id!,
						permission.Name!,
						permission.Description!,
						permission.Roles.Select(r => r.Name).ToList()!
					);
					permissionsResponse.Add(permissionResponse);
				}
				return permissionsResponse;
			}
		}


		/// <summary>
		/// Возвращает разрешения(с пагинацией) в виде списка объектов GetAllPermissionsResponse, 
		/// у которых название или описание содержат подстроку searchString.
		/// 2-ой параметр принимает номер страницы.
		/// 3-ий параметр принимает размер страницы.
		/// </summary>
		/// <param name="searchString"></param>
		/// <param name="pageNumber"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public List<GetAllPermissionsResponse> GetPermissions(string searchString, int pageNumber, int pageSize)
		{
			using (var context = new DatabaseContext())
			{
				var permissions = context.Permissions
				.AsNoTracking()
				.Include(p => p.Roles)
				.Where(p => p.Name!.Contains(searchString) || 
					p.Description!.Contains(searchString))
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToList();
				var permissionsResponse = new List<GetAllPermissionsResponse>();

				foreach (var permission in permissions)
				{
					var permissionResponse = new GetAllPermissionsResponse
					(
						permission.Id!,
						permission.Name!,
						permission.Description!,
						permission.Roles.Select(r => r.Name).ToList()!
					);
					permissionsResponse.Add(permissionResponse);
				}
				return permissionsResponse;
			}
		}


		/// <summary>
		/// Обновляет данные разрешения по его id.
		/// 1-ый параметр принимает новое название.
		/// 2-ой параметр принимает новое описание.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		public void UpdatePermission(int id, string name, string description)
		{
			using (var context = new DatabaseContext())
			{
				var permission = context.Permissions.First(x => x.Id == id);
				permission.Name = name;
				permission.Description = description;
				context.SaveChanges();
			}
		}
	}
}
