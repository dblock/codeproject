using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Event.Default;

namespace Vestris.Service.NHibernate
{
    /// <summary>
    /// NHibernate session factory.
    /// </summary>
    public class SessionFactory
    {
        private ISessionFactory _instance = null;
        private IInterceptor _interceptor = null;
        private SessionFactoryEventListeners _eventListeners;

        public SessionFactory()
        {
        }

        public SessionFactory(SessionFactoryEventListeners eventListeners)
        {
            _eventListeners = eventListeners;
        }

        //public SessionFactory(IInterceptor interceptor)
        //{
        //    _interceptor = interceptor;
        //}

        public ISessionFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    Configuration cfg = new Configuration();
                    cfg.Properties.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                    cfg.Properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                    cfg.Properties.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                    cfg.Properties.Add("connection.connection_string", "Server=localhost;initial catalog=OLS;Integrated Security=SSPI");
                    cfg.AddAssembly("Data.NHibernate");
                    if (_interceptor != null) cfg.Interceptor = _interceptor;
                    if (_eventListeners != null) _eventListeners.Configure(cfg);
                    _instance = cfg.BuildSessionFactory();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    }
}
