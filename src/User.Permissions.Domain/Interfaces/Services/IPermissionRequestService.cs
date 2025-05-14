using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Permissions.Domain.Entities;

namespace User.Permissions.Domain.Interfaces.Services
{
    public interface IPermissionRequestService
    {
        public Task<int> RequestPermissionAsync(Permission permission, CancellationToken cancellationToken = default);
    }
}
