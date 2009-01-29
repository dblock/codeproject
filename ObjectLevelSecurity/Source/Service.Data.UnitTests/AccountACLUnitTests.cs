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
    public class AccountACLUnitTests : ACLClassUnitTests
    {
        [Test]
        public void TestCreateDelete()
        {
            // an account can be created by anyone
            Account account = new Account();
            account.Created = DateTime.UtcNow;
            account.Name = Guid.NewGuid().ToString();
            account.Password = "password";
            Session.Save(account);
            Session.Flush();
            // switch context to self
            SessionManager.CurrentSessionContext = new UserContext(Session, account);
            // the owner can update his own account
            account.Name = Guid.NewGuid().ToString();
            Session.Save(account);
            // an account can be deleted by self
            Session.Delete(account);
            Session.Flush();
        }

        [Test, ExpectedException(typeof(AccessDeniedException))]
        public void TestCreateDeleteAnyone()
        {
            // an account can be created by anyone
            Account account = new Account();
            account.Created = DateTime.UtcNow;
            account.Name = Guid.NewGuid().ToString();
            account.Password = "password";
            Session.Save(account);
            Session.Flush();
            // an account cannot be deleted by guest
            try
            {
                Session.Delete(account);
            }
            finally
            {
                SessionManager.CurrentSessionContext = new UserContext(Session, account);
                Session.Delete(account);
            }
            Session.Flush();
        }
    }
}
