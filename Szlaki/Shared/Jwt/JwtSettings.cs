using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Szlaki.Shared
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public TimeSpan TokenTimeLife { get; internal set; }
    }
}
