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
	public class DatabaseContext : DbContext
	{
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Role> Roles { get; set; } = null!;
		public DbSet<Permission> Permissions { get; set; } = null!;
		public DbSet<DomainLogin> DomainLogins { get; set; } = null!;
		public DbSet<LocalLogin> LocalLogins { get; set; } = null!;

		public DatabaseContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TodoListDb;Username=postgres;Password=1234");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Установка конфигураций сущностей
			modelBuilder.ApplyConfiguration(new DomainLoginConfiguration());
			modelBuilder.ApplyConfiguration(new LocalLoginConfiguration());
			modelBuilder.ApplyConfiguration(new PermissionConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new RoleConfiguration());

			//Инициализация начальными данными ролей и разрешений
			modelBuilder.Entity<Role>().HasData(DataInitializer.GetRoles());
			modelBuilder.Entity<Permission>().HasData(DataInitializer.GetPermissions());
		}
	}
}
