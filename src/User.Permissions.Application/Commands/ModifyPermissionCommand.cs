using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Permissions.Application.Commands
{
    public class ModifyPermissionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
