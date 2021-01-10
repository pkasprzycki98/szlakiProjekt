using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Szlaki.Models
{
    public class Video
    {
        public string Id { get; set; }
        public string Path { get; set; }

        [ForeignKey("TrailId")]
        public Trail Trail { get; set; }
        public string TrailId { get; set; }
    }
}
