using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace Vestris.Service.NHibernate
{
    public interface ISessionContext
    {
        ISession Session { get; }
    }

    /// <summary>
    /// An empty session context.
    /// </summary>
    public class EmptySessionContext : ISessionContext
    {
        private ISession _session;

        public EmptySessionContext(ISession session)
        {
            _session = session;
        }

        public ISession Session
        {
            get
            {
                return _session;
            }
        }
    }
}
