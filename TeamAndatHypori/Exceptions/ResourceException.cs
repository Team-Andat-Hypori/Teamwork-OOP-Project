using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamAndatHypori.Exceptions
{
    using System;

    public class ResourcesNotFoundException : Exception
    {
        public ResourcesNotFoundException(string message)
            : base(message)
        {
        }
    }
}

