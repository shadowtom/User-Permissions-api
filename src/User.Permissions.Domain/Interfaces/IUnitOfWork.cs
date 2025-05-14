using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Permissions.Domain.Interfaces.Data;

namespace User.Permissions.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IPermissionRepository Permissions { get; }
        Task<int> CompleteAsync();
    }
}
