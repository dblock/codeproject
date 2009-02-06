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
    /// <summary>
    /// Execute tests under a user context.
    /// </summary>
    public class UserContextUnitTests : ACLClassUnitTests
    {
        protected Account _user = null;

        protected Account CreateUser()
        {
            using (new SessionManagerContextPusher(new GuestUserContext(Session)))
            {
                Account instance = new Account();
                instance.Created = DateTime.UtcNow;
                instance.Name = Guid.NewGuid().ToString();
                instance.Password = "password";
                Session.Save(instance);
                Session.Flush();
                return instance;
            }
        }

        public void DeleteUser(Account instance)
        {
            using (new SessionManagerContextPusher(new UserContext(Session, instance)))
            {
                Session.Delete(instance);
                Session.Flush();
            }
        }

        public override void SetUp()
        {
            base.SetUp();
            SessionManager.Initialize(new ThreadSessionSource(), new ServiceDataInterceptor());
            _user = CreateUser();
            SessionManager.CurrentSessionContext = new UserContext(Session, _user);
        }

        public override void TearDown()
        {
            DeleteUser(_user);
            _user = null;
            SessionManager.CurrentSessionContext = new GuestUserContext(Session);
            base.TearDown();
        }
    }
}
