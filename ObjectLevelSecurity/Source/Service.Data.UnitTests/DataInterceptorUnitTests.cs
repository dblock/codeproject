using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Vestris.Service.NHibernate;
using Vestris.Data.NHibernate;
using Vestris.Data.NHibernate.UnitTests;
using Vestris.Service.Identity;
using NHibernate;

namespace Vestris.Service.Data.UnitTests
{
    [TestFixture]
    public class DataInterceptorUnitTests : NHibernateTest
    {
        private ServiceDataInterceptor _interceptor = new ServiceDataInterceptor(new GuestUserContext());

        public override SessionFactory CreateSessionFactory()
        {
            return new SessionFactory(_interceptor);
        }

        [Test]
        public void TestCreateAccount()
        {
            Account account = new Account();
            account.Created = DateTime.UtcNow;
            account.Name = Guid.NewGuid().ToString();
            account.Password = "password";
            Session.Save(account);
            Session.Flush();
            _interceptor.UserContext = new UserContext(account);
            Session.Delete(account);
            Session.Flush();
        }
    }
}
