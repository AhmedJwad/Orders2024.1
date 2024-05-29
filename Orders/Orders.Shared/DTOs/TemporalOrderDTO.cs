using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.DTOs
{
    public class TemporalOrderDTO
    {
        public int ProductId { get; set; }
        public float Quantity { get; set; } = 1;
        public string Remarks { get; set; } = string.Empty;

    }
}
