using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using User.Permissions.Application.Commands;
using User.Permissions.Domain.Interfaces.Kafka;
using User.Permissions.Domain.Interfaces;
using User.Permissions.Domain.Interfaces.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using User.Permissions.Domain.Interfaces.Services;

namespace User.Permissions.Application.Handlers
{
    public class ModifyPermissionHandler : IRequestHandler<ModifyPermissionCommand,Unit>
    {
        private readonly IPermissionModificationService permissionModificationService;
        public ModifyPermissionHandler(IServiceProvider serviceProvider)
        {
            permissionModificationService = serviceProvider.GetRequiredService<IPermissionModificationService>();
        }

        public async Task<Unit> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            await permissionModificationService.ModifyPermissionAsync(
                request.Id,
                request.PermissionTypeId,
                request.PermissionDate,
                cancellationToken);

            return Unit.Value;
        }
    }
}
