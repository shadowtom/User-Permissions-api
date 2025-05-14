using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using User.Permissions.Application.DTOs;
using User.Permissions.Application.Querys;
using User.Permissions.Domain.Interfaces;
using User.Permissions.Domain.Interfaces.Kafka;
using User.Permissions.Domain.Interfaces.Services;

namespace User.Permissions.Application.Handlers
{
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
    {
        private readonly IPermissionGetService permissionGetService;
        public GetPermissionsHandler(IServiceProvider serviceProvider)
        {
            permissionGetService = serviceProvider.GetRequiredService<IPermissionGetService>();
        }
        public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await permissionGetService.getAllPermissions();

            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                EmployeeForeName = p.EmployeeForeName,
                EmployeeSurName = p.EmployeeSurName,
                PermissionType = p.PermissionType.Description,
                PermissionDate = p.PermissionDate
            });
        }
    }
}
