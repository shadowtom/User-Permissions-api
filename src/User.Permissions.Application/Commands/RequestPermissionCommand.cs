using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Permissions.Application.Commands
{
    public class RequestPermissionCommand : IRequest<int>
    {
        public string? EmployeeForeName { get; set; }
        public string? EmployeeSurName { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
