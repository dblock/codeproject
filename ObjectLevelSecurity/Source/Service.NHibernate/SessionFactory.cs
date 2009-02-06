using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

namespace Vestris.Service.NHibernate
{
    /// <summary>
    /// NHibernate session factory.
    /// </summary>
    public class SessionFactory
    {
        private ISessionFactory _instance = null;
        private IInterceptor _interceptor = null;

        public SessionFactory()
        {
        }

        public SessionFactory(IInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        public ISessionFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    Configuration cfg = new Configuration();
                    cfg.Properties.Add("hibernate.dialect", "NHibernate.Dialect.MsSql2000Dialect");
                    cfg.Properties.Add("hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                    cfg.Properties.Add("hibernate.connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                    cfg.Properties.Add("hibernate.connection.connection_string", "Server=localhost;initial catalog=OLS;Integrated Security=SSPI");
                    if (_interceptor != null) cfg.Interceptor = _interceptor;
                    cfg.AddAssembly("Data.NHibernate");
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
