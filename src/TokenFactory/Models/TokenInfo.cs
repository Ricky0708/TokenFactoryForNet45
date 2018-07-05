using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenFactory.Models
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public Dictionary<string,string> ExteneralInfo { get; set; }
    }
}
