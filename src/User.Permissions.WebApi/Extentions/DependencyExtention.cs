using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using User.Permissions.Application.Commands;
using User.Permissions.Application.Handlers;
using User.Permissions.Application.Querys;
using User.Permissions.Domain.Interfaces;
using User.Permissions.Domain.Interfaces.Data;
using User.Permissions.Domain.Interfaces.Elasticsearch;
using User.Permissions.Domain.Interfaces.Kafka;
using User.Permissions.Domain.Interfaces.Services;
using User.Permissions.Domain.Services;
using User.Permissions.Infrastructure.Data;
using User.Permissions.Infrastructure.Elasticsearch;
using User.Permissions.Infrastructure.Kafka;

namespace User.Permissions.WebApi.Extentions
{
    public static class DependencyExtention
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            var cstring = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<PermissionDbContext>(options =>
                options.UseSqlServer(cstring));

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ModifyPermissionCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(RequestPermissionCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ModifyPermissionHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(RequestPermissionHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetPermissionsQuery).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetPermissionsHandler).Assembly);
            });

            // Elasticsearch
            services.AddSingleton<IElasticClient>(sp =>
            {
                var settings = new ConnectionSettings(new Uri(configuration["ElasticSearch:Uri"]))
                    .DefaultIndex("permissions");

                return new ElasticClient(settings);
            });

            services.AddScoped<IElasticSearchRepository,ElasticSearchRepository>();


            services.AddSingleton<IPermissionRepository, PermissionRepository>();

            // Kafka
            services.AddSingleton<IKafkaProducer, KafkaProducer>();

            // Services
            services.AddTransient<IPermissionGetService, PermissionGetService>();
            services.AddTransient<IPermissionRequestService, PermissionRequestService>();
            services.AddTransient<IPermissionModificationService, PermissionModificationService>();

            return services;
        }
    }
}
