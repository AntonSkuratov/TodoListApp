using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Core.Entities;

namespace TodoListApp.Storage.Configurations
{
	public class RoleConfiguration : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.HasKey(r => r.Id);
			builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
			builder.Property(r => r.Description).IsRequired().HasMaxLength(1000);			
		}
	}
}
