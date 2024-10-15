using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApp.Core.Records.Requests
{
    public record CreatePermissionRequest(
        string Name,
        string Description);
}
