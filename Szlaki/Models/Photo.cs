using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Szlaki.Models
{
    public class Photo
    {
        public string Id { get; set; }
        public byte[] PhotoArray { get; set; }

        [ForeignKey("TrailId")]
        public Trail Trail { get; set; }
        public string TrailId { get; set; }
    }
}
