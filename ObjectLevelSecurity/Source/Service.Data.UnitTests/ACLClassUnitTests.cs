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
            base.SetUp();
            SessionManager.Initialize(new ThreadSessionSource(), new ServiceDataInterceptor());
            SessionManager.CurrentSessionContext = new GuestUserContext(Session);
        }

        public override void TearDown()
        {
            base.TearDown();
            SessionManager.CurrentSessionContext = null;
        }
    }
}
