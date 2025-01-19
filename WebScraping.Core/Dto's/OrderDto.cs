using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Core.Dto_s
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string TrackingNo { get; set; }

        public string Status { get; set; }
    }
}
