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
            // session source that is per user context, defaulting to guest
            UserContextSessionSource sessionSource = new UserContextSessionSource(new GuestUserContext());
            SessionManager.Initialize(sessionSource, ServiceDataEventListeners.Instance);
            SessionManager.BeginRequest();
        }

        public override void TearDown()
        {
            SessionManager.EndRequest();
            SessionManager.CurrentSessionContext = null;
        }
    }
}
