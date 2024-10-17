using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Core.Entities;
using TodoListApp.Storage.Configurations;
using TodoListApp.Storage.Repositories;

namespace TodoListApp.Storage
{
	public class TodoListContext : DbContext
	{
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Role> Roles { get; set; } = null!;
		public DbSet<Permission> Permissions { get; set; } = null!;
		public DbSet<Note> Notes { get; set; } = null!;
		public DbSet<DomainLogin> DomainLogins { get; set; } = null!;
		public DbSet<LocalLogin> LocalLogins { get; set; } = null!;

		public TodoListContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TodoListDb;Username=postgres;Password=1234");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new DomainLoginConfiguration());
			modelBuilder.ApplyConfiguration(new LocalLoginConfiguration());
			modelBuilder.ApplyConfiguration(new NoteConfiguration());
			modelBuilder.ApplyConfiguration(new PermissionConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new RoleConfiguration());

			modelBuilder.Entity<Role>().HasData(
				new Role { Id = 1, Name = "Admin", Description = "Администратор" },
				new Role { Id = 2, Name = "Watcher", Description = "Наблюдатель" },
				new Role { Id = 3, Name = "User", Description = "Пользователь" });

			modelBuilder.Entity<Permission>().HasData(
				new Permission { Id = 1, Name = "ModifyAccount", Description = "Редактирование профиля" },
				new Permission { Id = 2, Name = "CreateNewAccountNote", Description = "Создание заметок для текущего профиля" },
				new Permission { Id = 3, Name = "Get", Description = "Get" },
				new Permission { Id = 4, Name = "Post", Description = "Post" },
				new Permission { Id = 5, Name = "Put", Description = "Put" },
				new Permission { Id = 6, Name = "Delete", Description = "Delete" });
		}
	}
}
