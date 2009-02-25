using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Vestris.Service.NHibernate;

namespace Vestris.Service.Identity
{
    /// <summary>
    /// A threadsafe storage of a session per user context.
    /// </summary>
    public class UserContextSessionSource : ISessionStorage
    {
        private static Dictionary<ISessionContext, ISession> _sessionTable = new Dictionary<ISessionContext, ISession>();
        private static ISessionContext _sessionContext = null;

        /// <summary>
        /// A user-based session source.
        /// </summary>
        /// <param name="defaultUserContext">default user context</param>
        public UserContextSessionSource(ISessionContext defaultUserContext)
        {
            _sessionContext = defaultUserContext;
        }

        public ISession Session
        {
            get
            {
                lock (_sessionTable)
                {
                    ISession session = null;
                    if (! _sessionTable.TryGetValue(_sessionContext, out session))
                    {
                        session = SessionManager.Open();
                        _sessionTable[_sessionContext] = session;
                    }
                    return session;
                }
            }
            set
            {
                lock (_sessionTable)
                {
                    _sessionTable[_sessionContext] = value;
                }
            }
        }

        public ISessionContext Context
        {
            get
            {
                return _sessionContext;
            }
            set
            {
                if (value == null && _sessionContext != null)
                {
                    lock (_sessionTable)
                    {
                        _sessionTable.Remove(_sessionContext);
                    }
                }

                _sessionContext = value;
            }
        }
    }
}
