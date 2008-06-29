using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Vestris.ResourceLib
{
    public abstract class ResourceTable
    {
        public abstract void Write(BinaryWriter w);        
    }
}
