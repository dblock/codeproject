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
        public NHibernateTest()
        {

        }

        public ISession Session
        {
            get
            {
                return SessionManager.CurrentSession;
            }
            set
            {
                SessionManager.CurrentSession = value;
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            SessionManager.Initialize(new ThreadSessionSource(), null);
            SessionManager.BeginRequest();
        }

        [TearDown]
        public virtual void TearDown()
        {
            SessionManager.EndRequest();
        }
    }
}
