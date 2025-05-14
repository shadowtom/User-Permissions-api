using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Permissions.Domain.Entities
{
    public class kafkaLog
    {
        public Guid id { get; set; }
        public string? operationName { get;set; }    
        public string? operationData { get; set; }
    }
}
