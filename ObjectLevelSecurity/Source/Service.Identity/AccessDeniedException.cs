using System;
using System.Collections.Generic;
using System.Text;

namespace Vestris.Service.Identity
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException()
            : base("Access Denied")
        {
            Console.WriteLine("Throw: Access Denied");
        }
    }
}
