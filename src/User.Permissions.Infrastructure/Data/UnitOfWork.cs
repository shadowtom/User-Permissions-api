using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Permissions.Domain.Interfaces.Data;
using User.Permissions.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace User.Permissions.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PermissionDbContext _context;
        public IPermissionRepository Permissions { get; }

        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<PermissionDbContext>();
            Permissions = new PermissionRepository(serviceProvider);
        }

        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("{UtcNow} - Error saving the data in database - {Message}", DateTime.UtcNow,ex.Message);
                return 0;
            }
        }
    }
}
