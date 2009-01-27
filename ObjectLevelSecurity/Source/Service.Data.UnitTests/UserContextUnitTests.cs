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
            UserContext saved = CurrentUserContext;
            try
            {
                CurrentUserContext = new GuestUserContext();
                Account instance = new Account();
                instance.Created = DateTime.UtcNow;
                instance.Name = Guid.NewGuid().ToString();
                instance.Password = "password";
                Session.Save(instance);
                Session.Flush();
                return instance;
            }
            finally
            {
                CurrentUserContext = saved;
            }
        }

        public void DeleteUser(Account instance)
        {
            UserContext saved = CurrentUserContext;
            try
            {
                CurrentUserContext = new UserContext(instance);
                Session.Delete(instance);
                Session.Flush();
            }
            finally
            {
                CurrentUserContext = saved;
            }
        }

        public override void SetUp()
        {
            base.SetUp();
            _user = CreateUser();
            CurrentUserContext = new UserContext(_user);
        }

        public override void TearDown()
        {
            DeleteUser(_user);
            _user = null;
            base.TearDown();
        }
    }
}
