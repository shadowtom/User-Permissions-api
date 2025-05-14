using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using User.Permissions.Application.Commands;
using User.Permissions.Domain.Entities;
using User.Permissions.Domain.Interfaces;
using User.Permissions.Domain.Interfaces.Elasticsearch;
using User.Permissions.Domain.Interfaces.Kafka;
using User.Permissions.Domain.Interfaces.Services;

namespace User.Permissions.Application.Handlers
{
    public class RequestPermissionHandler : IRequestHandler<RequestPermissionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticSearchRepository _elastic;
        private readonly IKafkaProducer _kafka;
        private readonly IPermissionRequestService _requestPermissionService;

        public RequestPermissionHandler(IServiceProvider serviceProvider)
        {
            _requestPermissionService = serviceProvider.GetRequiredService<IPermissionRequestService>();
        }

        public async Task<int> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var permission = new Permission
                {
                    EmployeeForeName = request.EmployeeForeName,
                    EmployeeSurName = request.EmployeeSurName,
                    PermissionTypeId = request.PermissionTypeId,
                    PermissionDate = request.PermissionDate
                };
                return await _requestPermissionService.RequestPermissionAsync(permission, cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error("{UtcNow} - Error requesting permission - {Message}", DateTime.UtcNow, ex.Message);
                return 0;
            }
        }
    }
}
