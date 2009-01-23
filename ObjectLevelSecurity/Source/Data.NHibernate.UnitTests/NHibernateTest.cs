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
        private static SessionFactory _factory = new SessionFactory();
        private ISession _session;

        public NHibernateTest()
        {

        }

        public ISession Session
        {
            get
            {
                if (_session == null)
                {
                    _session = _factory.Instance.OpenSession();
                }

                return _session;
            }
            set
            {
                _session = value;
            }
        }

        [SetUp]
        public void SetUp()
        {
            _session = _factory.Instance.OpenSession();
        }

        [TearDown]
        public void TearDown()
        {
            _session.Close();
            _session = null;
        }
    }
}
