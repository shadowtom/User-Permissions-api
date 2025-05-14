using User.Permissions.Domain.Entities;

namespace User.Permissions.Domain.Interfaces.Services
{
    public interface IPermissionGetService
    {
        public Task<IEnumerable<Permission>> getAllPermissions();
    }
}