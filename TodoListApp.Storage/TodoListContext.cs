using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
			modelBuilder.ApplyConfiguration(new PermissionConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new RoleConfiguration());

			modelBuilder.Entity<Role>().HasData(DataInitializer.GetRoles());

			modelBuilder.Entity<Permission>().HasData(DataInitializer.GetPermissions());
		}
	}
}
