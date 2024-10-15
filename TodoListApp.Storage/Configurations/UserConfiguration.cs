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
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(u => u.Id);
			builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
			builder.Property(u => u.Lastname).IsRequired().HasMaxLength(100);
			builder.Property(u => u.Firstname).IsRequired().HasMaxLength(100);
			builder.Property(u => u.RefreshToken).IsRequired();
			builder.Property(u => u.IsBlocked).HasDefaultValue(false);
		}
	}
}
