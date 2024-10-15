using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Entities
{
	public class DomainLogin
	{
		public int Id { get; set; }
		public string? Login {  get; set; }
		public int UserId { get; set; }
		public User? User { get; set; }
	}
}
