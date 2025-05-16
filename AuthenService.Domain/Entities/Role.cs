using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenService.Domain.Entities
{
    public class Role : IdentityRole<long>
    {
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
