using System.Text.Json;
using User.Permissions.Domain.Interfaces.Elasticsearch;
using User.Permissions.Domain.Interfaces.Kafka;
using User.Permissions.Domain.Interfaces;
using User.Permissions.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using User.Permissions.Domain.Entities;
using Serilog;

namespace User.Permissions.Domain.Services
{
    public class PermissionModificationService : IPermissionModificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticSearchRepository _elastic;
        private readonly IKafkaProducer _kafka;

        public PermissionModificationService(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            _elastic = serviceProvider.GetRequiredService<IElasticSearchRepository>();
            _kafka = serviceProvider.GetRequiredService<IKafkaProducer>();
        }

        public async Task ModifyPermissionAsync(
            int permissionId,
            int permissionTypeId,
            DateTime permissionDate,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (permissionId <= 0)
                    throw new ArgumentException("Permission ID must be greater than zero.", nameof(permissionId));
                var permission = await _unitOfWork.Permissions.GetByIdAsync(permissionId);
                if (permission == null)
                    throw new KeyNotFoundException($"Permission with ID {permissionId} not found.");

                permission.PermissionTypeId = permissionTypeId;
                permission.PermissionDate = permissionDate;

                _unitOfWork.Permissions.Update(permission);
                await _unitOfWork.CompleteAsync();

                await _elastic.IndexPermissionAsync(permission);

                await _kafka.ProduceAsync(new kafkaLog() { id = Guid.NewGuid(), operationName = "modify", operationData = JsonSerializer.Serialize(permission) });
            }
            catch (Exception ex)
            {
                Log.Error("{UtcNow} - Error modifying permission - {Message}", DateTime.UtcNow, ex.Message);
            }
        }
    }
}
