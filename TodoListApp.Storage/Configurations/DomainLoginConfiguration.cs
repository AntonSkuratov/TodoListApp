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
	public class DomainLoginConfiguration : IEntityTypeConfiguration<DomainLogin>
	{
		public void Configure(EntityTypeBuilder<DomainLogin> builder)
		{
			builder.HasKey(d => d.Id);
			builder.HasAlternateKey(l => l.Login);
			builder.Property(d => d.Login).IsRequired().HasMaxLength(100);
		}
	}
}
