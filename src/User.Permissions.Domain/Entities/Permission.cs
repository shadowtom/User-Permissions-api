using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Permissions.Domain.Entities
{
    [Table("Permissions")]
    public class Permission
    {
        public int Id { get; set; }
        public string EmployeeForeName { get; set; }
        public string EmployeeSurName { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }
        public PermissionType PermissionType { get; set; }
    }
}
