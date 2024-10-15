using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Records.Requests
{
    public record CreateUserRequest(
        string Username,
        string Lastname,
        string Firstname,
        string Login,
        string Password);
}
