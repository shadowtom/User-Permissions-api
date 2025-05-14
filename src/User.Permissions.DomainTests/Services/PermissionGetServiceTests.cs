using Microsoft.VisualStudio.TestTools.UnitTesting;
using User.Permissions.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using User.Permissions.Domain.Interfaces.Kafka;
using User.Permissions.Domain.Interfaces;
using Moq;
using User.Permissions.Domain.Entities;

namespace User.Permissions.Domain.Services.Tests
{
    [TestClass()]
    public class PermissionGetServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ServiceProvider _serviceProvider;

        public PermissionGetServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            Mock<IKafkaProducer> _kafkaProducerMock = new Mock<IKafkaProducer>();



            var services = new ServiceCollection();
            services.AddSingleton(_unitOfWorkMock.Object);
            services.AddSingleton(_kafkaProducerMock.Object);
            _serviceProvider = services.BuildServiceProvider();
        }
        // Fuente de datos dinámica para los escenarios
        public static IEnumerable<object[]> GetAllPermissionsScenarios()
        {
            yield return new object[]
            {
                new List<Permission>
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
                }
            };

            yield return new object[]
            {
                new List<Permission>() // Sin permisos
            };

            yield return new object[]
            {
                new List<Permission>
                {
                    new Permission
                    {
                        Id = 2,
                        EmployeeForeName = "Ana",
                        EmployeeSurName = "García",
                        PermissionTypeId = 2,
                        PermissionDate = DateTime.Today,
                        PermissionType = new PermissionType { Id = 2, Description = "Enfermedad" }
                    },
                    new Permission
                    {
                        Id = 3,
                        EmployeeForeName = "Luis",
                        EmployeeSurName = "Martínez",
                        PermissionTypeId = 1,
                        PermissionDate = DateTime.Today,
                        PermissionType = new PermissionType { Id = 1, Description = "Vacaciones" }
                    }
                }
            };
        }

        [TestMethod]
        [DynamicData(nameof(GetAllPermissionsScenarios), DynamicDataSourceType.Method)]
        public async Task GetAllPermissionsTest(IEnumerable<Permission> permissions)
        {

            // Arrange
            _unitOfWorkMock.Setup(u => u.Permissions.GetAllAsync())
                .ReturnsAsync(permissions);

            var service = new PermissionGetService(_serviceProvider);

            // Act
            var result = await service.getAllPermissions();

            // Assert
            CollectionAssert.AreEquivalent(permissions.ToList(), result.ToList());
        }
    }
}