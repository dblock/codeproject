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
            // create a new account, this can be done by anyone
            Account account = new Account();
            account.Created = DateTime.UtcNow;
            account.Name = Guid.NewGuid().ToString();
            account.Password = "password";
            Session.Save(account);
            Session.Flush();

            // switch context to self and delete the account, this can be done by the account owner himself
            using (new SessionManagerContextPusher(new UserContext(account)))
            {
                Account accountCopy = Session.Load<Account>(account.Id);
                // the owner can update his own account
                accountCopy.Name = Guid.NewGuid().ToString();
                Session.Save(accountCopy);
                // an account can be deleted by self
                Session.Delete(accountCopy);
                Session.Flush();
            }
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
                Session.Flush();
            }
            finally
            {
                using (new SessionManagerContextPusher(new UserContext(account)))
                {
                    Account accountCopy = Session.Load<Account>(account.Id);
                    Session.Delete(accountCopy);
                    Session.Flush();
                }
            }
        }

        //[Test]
        //public void TestRetrieveAccountPassword()
        //{
        //    Account account = new Account();
        //    account.Created = DateTime.UtcNow;
        //    account.Name = Guid.NewGuid().ToString();
        //    account.Password = "password";
        //    Session.Save(account);
        //    Session.Flush();
        //    Session.Clear();
        //    // an account can be loaded by anyone, but the password is blanked
        //    Account accountcopy1 = Session.Load<Account>(account.Id);
        //    Assert.AreEqual(account.Id, accountcopy1.Id);
        //    Assert.IsTrue(string.IsNullOrEmpty(accountcopy1.Password));
        //    SessionManager.CurrentSessionContext = new UserContext(Session, account);
        //    // an account can be loaded by user, with the password
        //    Account accountcopy2 = Session.Load<Account>(account.Id);
        //    Assert.AreEqual(account.Id, accountcopy2.Id);
        //    Assert.AreEqual(account.Password, accountcopy2.Password);
        //    // delete
        //    Session.Delete(account);
        //    Session.Flush();
        //}
    }
}
