using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenFactory.Xuenn
{
    public class TokenValidateOption
    {
        public bool IsValidExpired { get; set; }
        public bool validateOTT { get; set; }
        public string CommonCSNTokenKey { get; set; }
        public string CommonCSNTokenIV { get; set; }
    }
}
