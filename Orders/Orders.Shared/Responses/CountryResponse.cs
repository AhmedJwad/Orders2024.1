using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Responses
{
    public class CountryResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Iso2 { get; set; }

    }
}
