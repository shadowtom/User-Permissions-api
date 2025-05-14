using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Permissions.Application.DTOs;

namespace User.Permissions.Application.Querys
{
    public class GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>> { }

}
