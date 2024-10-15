using Microsoft.EntityFrameworkCore;
using TodoListApp.Core.Entities;
using TodoListApp.Core.Infrastructure.Security;
using TodoListApp.Storage;
using TodoListApp.Storage.Repositories;

User Get(string pass)
{
	TodoListContext context = new TodoListContext();
	var user = context.Users.Include(u => u.LocalLogin)
		.First(x => x.LocalLogin!.PasswordHash == PasswordHandler.GenerateHash(pass, "OPH3blY4Vg76nvzOxcEuaw=="));
	return user;
}

Console.WriteLine(Get("bebra123").Username);
