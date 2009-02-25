using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Event;

namespace Vestris.Service.NHibernate
{
    /// <summary>
    /// A session manager exposing the NHibernate session and an additional context.
    /// </summary>
    public abstract class SessionManager
    {
        private static object _locker = new object();
        private static ISessionStorage _sessionSource = null;
        private static SessionFactory _sessionFactory = null;

        //public static void Initialize(ISessionStorage storage, IInterceptor interceptor)
        //{
        //    _sessionSource = storage;
        //    _sessionFactory = new SessionFactory(interceptor);
        //}

        public static void Initialize(ISessionStorage storage, SessionFactoryEventListeners eventListeners)
        {
            _sessionSource = storage;
            _sessionFactory = new SessionFactory(eventListeners);            
        }

        private static SessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    lock (_locker)
                    {
                        if (_sessionFactory == null)
                        {
                            _sessionFactory = new SessionFactory();
                        }
                    }
                }
                return _sessionFactory;
            }
        }

        private static ISessionStorage SessionSource
        {
            get
            {
                if (_sessionSource == null)
                {
                    lock (_locker)
                    {
                        if (_sessionSource == null)
                        {
                            _sessionSource = new ThreadSessionSource();
                        }
                    }
                }

                return _sessionSource;
            }
            set
            {
                lock (_locker)
                {
                    _sessionSource = value;
                }
            }
        }

        public static ISessionContext CurrentSessionContext
        {
            get
            {
                return SessionSource.Context;
            }
            set
            {
                SessionSource.Context = value;
            }
        }

        /// <summary>
        /// Initializes internal state for handling new request. Call this method at 
        /// the beginning of the ASP.NET request.
        /// </summary>
        public static void BeginRequest()
        {
            //if (HasCurrent())
            //{
            //    throw new ApplicationException("There mustn't be current session at the begining of a request.");
            //}
        }

        /// <summary>
        /// Finalizes the request. If current session exists the method flushes it and
        /// closes. Call this method at the end of ASP.NET request.
        /// </summary>
        public static void EndRequest()
        {
            if (HasCurrent())
                Close();

            SessionSource.Session = null;
        }

        public static ISession Open()
        {
            ISession s = SessionFactory.Instance.OpenSession();
            s.FlushMode = FlushMode.Never;
            return s;
        }

        /// <summary>
        /// Returns a current session object.
        /// </summary>
        /// <remarks>The session object will be created upon first request for it.</remarks>
        public static ISession CurrentSession
        {
            get
            {
                ISession s = SessionSource.Session;
                if (s == null)
                {
                    s = Open();
                    SessionSource.Session = s;
                }
                return s;
            }
            set
            {
                SessionSource.Session = value;
            }
        }

        /// <summary>
        /// Tells if we currently have a session object or not.
        /// </summary>
        /// <returns><b>true</b> if there is a current session and <b>false</b> otherwise.</returns>
        private static bool HasCurrent()
        {
            return SessionSource.Session != null;
        }

        /// <summary>
        /// Flushes and closes current session.
        /// </summary>
        public static void CloseAndFlush()
        {
            ISession session = SessionSource.Session;
            if (session != null)
            {
                session.Flush();
                session.Close();
                SessionSource.Session = null;
            }
        }

        /// <summary>
        /// Closes current session.
        /// </summary>
        /// <remarks><b>N.B.</b> Close do not flush the session.</remarks>
        public static void Close()
        {
            ISession session = SessionSource.Session;
            if (session != null)
            {
                session.Close();
                session.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                SessionSource.Session = null;
            }
        }

        /// <summary>
        /// Flushes current session.
        /// </summary>
        public static void Flush()
        {
            ISession session = SessionSource.Session;
            if (session != null)
                session.Flush();
        }
    }
}
