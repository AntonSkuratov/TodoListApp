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
	public class LocalLoginConfiguration : IEntityTypeConfiguration<LocalLogin>
	{
		public void Configure(EntityTypeBuilder<LocalLogin> builder)
		{
			builder.HasKey(l => l.Id);
			builder.HasAlternateKey(l => l.Login);
			builder.Property(l => l.Login).IsRequired().HasMaxLength(100);
			builder.Property(l => l.PasswordHash).IsRequired();
			builder.Property(l => l.Salt).IsRequired();
		}
	}
}
