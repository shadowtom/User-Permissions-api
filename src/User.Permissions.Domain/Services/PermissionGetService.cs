using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using User.Permissions.Domain.Entities;
using User.Permissions.Domain.Interfaces;
using User.Permissions.Domain.Interfaces.Data;
using User.Permissions.Domain.Interfaces.Kafka;
using User.Permissions.Domain.Interfaces.Services;

namespace User.Permissions.Domain.Services
{
    public class PermissionGetService : IPermissionGetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKafkaProducer _kafka;
        public PermissionGetService(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            _kafka = serviceProvider.GetRequiredService<IKafkaProducer>();

        }
        public async Task<IEnumerable<Permission>> getAllPermissions()
        {
            try
            {

                var permissions = await _unitOfWork.Permissions.GetAllAsync();

                await _kafka.ProduceAsync(new kafkaLog() { id = Guid.NewGuid(), operationData = null, operationName = "getAll" });

                return permissions;
            }
            catch (Exception ex)
            {
                Log.Error("{UtcNow} - Error getting the permissions - {Message}", DateTime.UtcNow, ex.Message);
                return new List<Permission>();
            }

        }
    }
}
