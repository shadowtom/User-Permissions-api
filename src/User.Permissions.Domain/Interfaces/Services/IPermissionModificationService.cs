namespace User.Permissions.Domain.Interfaces.Services
{
    public interface IPermissionModificationService
    {
        public Task ModifyPermissionAsync(
            int permissionId,
            int permissionTypeId,
            DateTime permissionDate,
            CancellationToken cancellationToken = default);
    }
}