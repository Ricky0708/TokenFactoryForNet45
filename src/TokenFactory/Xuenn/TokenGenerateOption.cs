using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenFactory.Xuenn
{
    public class TokenGenerateOption
    {
        public int Expirty { get; set; }
        public string CommonCSNTokenKey { get; set; }
        public string CommonCSNTokenIV { get; set; }
    }
}
