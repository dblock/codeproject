using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace Vestris.Service.NHibernate
{
    /// <summary>
    /// A session manager exposing the NHibernate session and an additional context.
    /// </summary>
    public abstract class SessionManager
    {
        private static ISession _currentSession = null;
        private static ISessionContext _currentSessionContext = null;

        public static ISession CurrentSession
        {
            get
            {
                return _currentSession;
            }
            set
            {
                _currentSession = value;
            }
        }

        public static ISessionContext CurrentSessionContext
        {
            get
            {
                return _currentSessionContext;
            }
            set
            {
                _currentSessionContext = value;
            }
        }
    }
}
