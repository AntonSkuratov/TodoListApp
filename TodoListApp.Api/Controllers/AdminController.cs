﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Storage.Repositories;

namespace TodoListApp.Api.Controllers
{
	[Authorize(Roles = "Admin,Watcher")]
	public class AdminController : Controller
	{
		[HttpGet]
		[Authorize(Policy = "GetPermission")]
		[Route("/Admin/GetAllUser")]
		public IActionResult GetAllUser()
		{
			return Ok(new UserRepository().GetAllUsers());
		}

		[Authorize(Policy = "DeletePermission")]
		[HttpDelete]
		[Route("/Admin/DeleteUserById")]
		public IActionResult DeleteUserById(int userId)
		{
			new UserRepository().DeleteUser(userId);
			return Ok();
		}

		[Authorize(Policy = "PutPermission")]
		[HttpPut]
		[Route("/Admin/ChangeUserBlockingStatus")]
		public IActionResult ChangeUserBlockingStatus(int userId)
		{
			new UserRepository().ChangeBlockingStatus(userId);
			return Ok();
		}

		[Authorize(Policy = "GetPermission")]
		[HttpGet]
		[Route("/Admin/SearchUsers")]
		public IActionResult SearchUsers(string searchString)
		{
			return Ok(new UserRepository().SearchUsers(searchString));
		}

		[Authorize(Policy = "GetPermission")]
		[HttpGet]
		[Route("/Admin/GetUserById")]
		public IActionResult GetUserById(int id)
		{
			return Ok(new UserRepository().GetUser(id));
		}

		[Authorize(Policy = "PutPermission")]
		[HttpPut]
		[Route("/Admin/AddRole")]
		public IActionResult AddRole(int userId, int roleId)
		{
			new UserRepository().AddRole(userId, roleId);
			return Ok();
		}

		[Authorize(Policy = "PutPermission")]
		[HttpPut]
		[Route("/Admin/DeleteRole")]
		public IActionResult DeleteRole(int userId, int roleId)
		{
			new UserRepository().DeleteRole(userId, roleId);
			return Ok();
		}

		[Authorize(Policy = "GetPermission")]
		[HttpGet]
		[Route("/Admin/GetAllNotes")]
		public IActionResult GetAllNotes()
		{
			var notes = new NoteRepository().GetAllNotes();
			return Ok(notes);
		}
	}
}