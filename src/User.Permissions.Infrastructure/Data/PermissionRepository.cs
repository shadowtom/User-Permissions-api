using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Permissions.Domain.Entities;
using User.Permissions.Domain.Interfaces.Data;

namespace User.Permissions.Infrastructure.Data
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly PermissionDbContext _context;
        public PermissionRepository(IServiceProvider serviceProvider)
        {
            _context =serviceProvider.GetRequiredService<PermissionDbContext>();
        }

        public async Task<Permission?> GetByIdAsync(int id)
        {
            try
            {

                return await _context.Permissions.Include(p => p.PermissionType)
                                                 .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                Log.Error("{UtcNow} - Error getting the permission by id - {Message}", DateTime.UtcNow, ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            try
            {

                return await _context.Permissions.Include(p => p.PermissionType).ToListAsync() ?? new List<Permission>();
            }
            catch (Exception ex)
            {
                Log.Error("{UtcNow} - Error getting the permissions - {Message}", DateTime.UtcNow, ex.Message);
                return new List<Permission>();
            }
        }

        public async Task AddAsync(Permission permission)
        {
            try
            {
                await _context.Permissions.AddAsync(permission);
            }
            catch (Exception ex)
            {
                Log.Error("{UtcNow} - Error adding the permission - {Message}", DateTime.UtcNow, ex.Message);
            }
        }

        public void Update(Permission permission)
        {
            try
            {

                _context.Permissions.Update(permission);
            }
            catch (Exception ex)
            {
                Log.Error("{UtcNow} - Error updating the permission - {Message}", DateTime.UtcNow, ex.Message);
            }
        }
    }
}
