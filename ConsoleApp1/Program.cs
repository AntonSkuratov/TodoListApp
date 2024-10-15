using Microsoft.EntityFrameworkCore;
using TodoListApp.Core.Entities;
using TodoListApp.Storage;

TodoListContext context = new TodoListContext();
foreach(User user in context.Users.Include(u => u.DomainLogin).ToList())
{
	Console.WriteLine(user.Firstname + "  " + user.DomainLogin!.Login);
}