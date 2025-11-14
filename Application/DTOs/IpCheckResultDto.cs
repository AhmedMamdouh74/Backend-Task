using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class IpCheckResultDto
    {

        public string IpAddress { get; set; }
        public string CountryCode { get; set; }
        public bool IsBlocked { get; set; }

    }
}
