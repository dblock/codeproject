using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Service.NHibernate;
using Vestris.Data.NHibernate;
using Vestris.Data.NHibernate.UnitTests;
using Vestris.Service.Identity;
using NHibernate;
using NUnit.Framework;

namespace Vestris.Service.Data.UnitTests
{
    public class ACLClassUnitTests : NHibernateTest
    {
        public override void SetUp()
        {
            SessionManager.CurrentSessionContext = new GuestUserContext(Session);
            base.SetUp();
        }

        public override void TearDown()
        {
            base.TearDown();
            SessionManager.CurrentSessionContext = null;
        }

        public override SessionFactory CreateSessionFactory()
        {
            return new SessionFactory(new ServiceDataInterceptor());
        }
    }
}
