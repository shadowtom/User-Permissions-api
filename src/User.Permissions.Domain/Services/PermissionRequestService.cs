using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Permissions.Domain.Interfaces.Kafka;
using User.Permissions.Domain.Interfaces;
using User.Permissions.Domain.Interfaces.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using User.Permissions.Domain.Entities;
using Serilog;
using User.Permissions.Domain.Interfaces.Services;

namespace User.Permissions.Domain.Services
{
    public class PermissionRequestService : IPermissionRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticSearchRepository _elastic;
        private readonly IKafkaProducer _kafka;
        public PermissionRequestService(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            _elastic = serviceProvider.GetRequiredService<IElasticSearchRepository>();
            _kafka = serviceProvider.GetRequiredService<IKafkaProducer>();
        }
        public async Task<int> RequestPermissionAsync(Permission permission, CancellationToken cancellationToken = default)
        {
            try
            {

                await _unitOfWork.Permissions.AddAsync(permission);
                await _unitOfWork.CompleteAsync();

                await _elastic.IndexPermissionAsync(permission);

                await _kafka.ProduceAsync(new kafkaLog()
                {
                    id = Guid.NewGuid(),
                    operationName = "request",
                    operationData = JsonSerializer.Serialize(permission)
                });

                return permission.Id;
            }
            catch (Exception ex)
            {
                Log.Error("{UtcNow} - Error requesting permission - {Message}", DateTime.UtcNow, ex.Message);
                return 0;
            }
        }

    }
}
