using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Vestris.Data.NHibernate.UnitTests;

namespace Vestris.Service.Identity.UnitTests
{
    [TestFixture]
    public class IdentityServiceUnitTests : NHibernateTest
    {
        [Test, ExpectedException(typeof(AccessDeniedException))]
        public void InvalidLoginTest()
        {
            IdentityService service = new IdentityService(Session);
            service.Login(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        [Test]
        public void ValidLoginTest()
        {
            AccountUnitTests _account = new AccountUnitTests();
            _account.Create();
            try
            {
                IdentityService service = new IdentityService(Session);
                UserContext userContext = service.Login(_account.Instance.Name, _account.Instance.Password);
                Assert.AreEqual(userContext.AccountId, _account.Instance.Id);
                Assert.GreaterOrEqual(DateTime.UtcNow, userContext.TimeStamp);
            }
            finally
            {
                _account.Delete();
            }
        }
    }
}
