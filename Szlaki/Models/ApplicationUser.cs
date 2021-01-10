using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Szlaki.Models
{
    public class ApplicationUser : IdentityUser
    {
       public DateTime CraeteDateTime { get; set; }
       public DateTime LastModifedDateTime { get; set; }
    }
}
