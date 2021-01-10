using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Szlaki.Models
{
    public class FileDto
    {
        public string TrailId { get; set; }
        public IFormFile TrailPhoto { get; set; }
    }
}
