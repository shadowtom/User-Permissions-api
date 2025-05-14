using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Permissions.Application.DTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string? EmployeeForeName { get; set; }
        public string? EmployeeSurName { get; set; }
        public string? PermissionType { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
