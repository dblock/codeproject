using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace Vestris.Service.NHibernate
{
    public interface ISessionContext
    {
    }

    /// <summary>
    /// An empty session context.
    /// </summary>
    public class EmptySessionContext : ISessionContext
    {
        public EmptySessionContext()
        {
        }
    }
}
