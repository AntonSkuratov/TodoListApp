﻿using Microsoft.EntityFrameworkCore;
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
		public void CreatePermission(CreatePermissionRequest permissionRequest)
		{
			using (var context=new TodoListContext())
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

		public void DeletePermission(int id)
		{
			using (var context = new TodoListContext())
			{
				var permission = context.Permissions.First(x => x.Id == id);
				context.Permissions.Remove(permission);
				context.SaveChanges();
			}			
		}

		public List<GetAllPermissionsResponse> GetAllPermissions()
		{
			using (var context = new TodoListContext())
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

		public List<GetAllPermissionsResponse> GetPermissions(int pageNumber, int pageSize)
		{
			using (var context = new TodoListContext())
			{
				var permissions = context.Permissions
				.AsNoTracking()
				.Include(p => p.Roles)
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

		public void UpdatePermission(int id, string name, string description)
		{
			using (var context = new TodoListContext())
			{
				var permission = context.Permissions.First(x => x.Id == id);
				permission.Name = name;
				permission.Description = description;
				context.SaveChanges();
			}
		}
	}
}