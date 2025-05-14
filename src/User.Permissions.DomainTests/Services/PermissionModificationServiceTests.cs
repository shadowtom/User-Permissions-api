using Microsoft.VisualStudio.TestTools.UnitTesting;
using User.Permissions.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using User.Permissions.Domain.Interfaces;
using User.Permissions.Domain.Interfaces.Kafka;
using User.Permissions.Domain.Entities;

namespace User.Permissions.Domain.Services.Tests
{
    [TestClass()]
    public class PermissionModificationServiceTests
    {

        private Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ServiceProvider _serviceProvider;
        public PermissionModificationServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IKafkaProducer> _kafkaProducerMock = new Mock<IKafkaProducer>();



            var services = new ServiceCollection();
            services.AddSingleton(_unitOfWorkMock.Object);
            services.AddSingleton(_kafkaProducerMock.Object);
            _serviceProvider = services.BuildServiceProvider();
        }
        public static IEnumerable<object[]> ModifyPermissionScenarios()
        {
            yield return new object[]
            {
                new Permission
                {
                    Id = 1,
                    EmployeeForeName = "Juan",
                    EmployeeSurName = "Pérez",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.Today,
                    PermissionType = new PermissionType { Id = 1, Description = "Vacaciones" }
                }
            };

            yield return new object[]
            {
                new Permission
                {
                    Id = 2,
                    EmployeeForeName = "Ana",
                    EmployeeSurName = "García",
                    PermissionTypeId = 2,
                    PermissionDate = DateTime.Today.AddDays(-1),
                    PermissionType = new PermissionType { Id = 2, Description = "Enfermedad" }
                }
            };
        }

        [TestMethod()]
        [DynamicData(nameof(ModifyPermissionScenarios), DynamicDataSourceType.Method)]
        public void ModifyPermissionAsyncTest(Permission permission)
        {
            var service = new PermissionModificationService(_serviceProvider);

            var result = service.ModifyPermissionAsync(permission.Id,permission.PermissionTypeId,permission.PermissionDate);

            Assert.Fail();
        }
    }
}