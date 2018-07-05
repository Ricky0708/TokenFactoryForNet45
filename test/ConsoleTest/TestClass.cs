using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TokenFactory;

namespace ConsoleTest
{
    public class TestClass : baseValidateModel
    {
        public TestClass(ClaimsPrincipal principal) : base(principal) { }
    }
}
