using Microsoft.VisualStudio.TestTools.UnitTesting;
using User.Permissions.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Net;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace User.Permissions.WebApi.Controllers.Tests
{
    [TestClass()]
    public class PermissionsControllerTests
    {
        private static WebApplicationFactory<Program> _factory;
        private static HttpClient _client;

        public static void ClassInit(TestContext context)
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }
        public static IEnumerable<object[]> RequestPermissionScenarios()
        {
            yield return new object[]
            {
                new
                {
                    EmployeeForeName = "Juan",
                    EmployeeSurName = "Pérez",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.Today
                },
                HttpStatusCode.OK // O el código esperado según tu lógica
            };

            yield return new object[]
            {
                new
                {
                    EmployeeForeName = "",
                    EmployeeSurName = "SinNombre",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.Today
                },
                HttpStatusCode.BadRequest // Por ejemplo, si el nombre es obligatorio
            };
        }

        [TestMethod]
        [DynamicData(nameof(RequestPermissionScenarios), DynamicDataSourceType.Method)]
        public async Task RequestPermissionTest(object request, HttpStatusCode expectedStatus)
        {
            // Arrange
            var url = "/api/permissions/request"; // Ajusta la ruta según tu controlador

            // Act
            var response = await _client.PostAsJsonAsync(url, request);

            // Assert
            Assert.AreEqual(expectedStatus, response.StatusCode);
        }

        public static IEnumerable<object[]> ModifyPermissionScenarios()
        {
            yield return new object[]
            {
                new
                {
                    Id = 1,
                    EmployeeForeName = "Juan",
                    EmployeeSurName = "Pérez",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.Today
                },
                HttpStatusCode.OK // O el código esperado según tu lógica
            };

            yield return new object[]
            {
                new
                {
                    Id = 9999, // Id inexistente
                    EmployeeForeName = "Ana",
                    EmployeeSurName = "García",
                    PermissionTypeId = 2,
                    PermissionDate = DateTime.Today
                },
                HttpStatusCode.NotFound // Por ejemplo, si el permiso no existe
            };

            yield return new object[]
            {
                new
                {
                    Id = 2,
                    EmployeeForeName = "",
                    EmployeeSurName = "SinNombre",
                    PermissionTypeId = 1,
                    PermissionDate = DateTime.Today
                },
                HttpStatusCode.BadRequest // Si el nombre es obligatorio
            };
        }

        [TestMethod]
        [DynamicData(nameof(ModifyPermissionScenarios), DynamicDataSourceType.Method)]
        public async Task ModifyPermissionTest(object request, HttpStatusCode expectedStatus)
        {
            // Arrange
            var url = "/api/permissions/modify"; // Ajusta la ruta según tu controlador

            // Act
            var response = await _client.PutAsJsonAsync(url, request);

            // Assert
            Assert.AreEqual(expectedStatus, response.StatusCode);
        }

        public static IEnumerable<object[]> GetPermissionsScenarios()
        {
            yield return new object[]
            {
                HttpStatusCode.OK // Espera éxito cuando hay permisos
            };
            // Puedes agregar más escenarios si tu endpoint devuelve otros códigos
        }

        [TestMethod]
        [DynamicData(nameof(GetPermissionsScenarios), DynamicDataSourceType.Method)]
        public async Task GetPermissionsTest(HttpStatusCode expectedStatus)
        {
            // Arrange
            var url = "/api/permissions"; // Ajusta la ruta según tu controlador

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.AreEqual(expectedStatus, response.StatusCode);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var permissions = await response.Content.ReadFromJsonAsync<List<object>>();
                Assert.IsNotNull(permissions);
                // Puedes agregar más aserciones según la estructura esperada
            }
        }
    }
}