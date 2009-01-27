using System;
using NUnit.Framework;
using NHibernate;
using NHibernate.Cfg;
using Vestris.Service.NHibernate;

namespace Vestris.Data.NHibernate.UnitTests
{
    /// <summary>
    /// NHibernate test foundation.
    /// </summary>
    public class NHibernateTest
    {
        protected ISession _session = null;
        protected SessionFactory _factory = null;

        public NHibernateTest()
        {

        }

        public ISession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = Factory.OpenSession();
                }

                return _session;
            }
            set
            {
                _session = value;
            }
        }

        public virtual SessionFactory CreateSessionFactory()
        {
            return new SessionFactory(null);
        }

        public ISessionFactory Factory
        {
            get
            {
                return _factory.Instance;
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            _factory = CreateSessionFactory(); 
            _session = Factory.OpenSession();
        }

        [TearDown]
        public virtual void TearDown()
        {
            _session.Close();
            _session = null;
        }
    }
}
