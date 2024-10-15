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
	public class NoteConfiguration : IEntityTypeConfiguration<Note>
	{
		public void Configure(EntityTypeBuilder<Note> builder)
		{
			builder.HasKey(n => n.Id);
			builder.Property(n => n.Title).IsRequired().HasMaxLength(100);
			builder.Property(n => n.Description).IsRequired().HasMaxLength(1000);
		}
	}
}
