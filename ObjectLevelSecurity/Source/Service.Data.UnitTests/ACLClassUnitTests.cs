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
        private ServiceDataInterceptor _interceptor = null;

        public override SessionFactory CreateSessionFactory()
        {
            return new SessionFactory(_interceptor);
        }

        public override void SetUp()
        {
            _interceptor = new ServiceDataInterceptor(new GuestUserContext());
            base.SetUp();
        }

        public UserContext CurrentUserContext
        {
            get
            {
                return _interceptor.UserContext;
            }
            set
            {
                _interceptor.UserContext = value;
            }
        }
    }
}
