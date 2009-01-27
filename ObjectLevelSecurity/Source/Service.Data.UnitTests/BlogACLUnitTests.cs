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
    public class BlogACLUnitTests : UserContextUnitTests
    {
        [Test]
        public void TestCreateDelete()
        {
            Blog blog = new Blog();
            blog.Account = _user;
            blog.Created = DateTime.UtcNow;
            blog.Description = Guid.NewGuid().ToString();
            blog.Name = Guid.NewGuid().ToString();
            Session.Save(blog);
            Session.Delete(blog);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestTakeOwnershipDisallowed()
        {
            Blog blog = new Blog();
            blog.Account = _user;
            blog.Created = DateTime.UtcNow;
            blog.Description = Guid.NewGuid().ToString();
            blog.Name = Guid.NewGuid().ToString();
            Session.Save(blog);
            Session.Flush();
            Account user2 = CreateUser();
            UserContext saved = CurrentUserContext;
            try
            {
                CurrentUserContext = new UserContext(user2);
                blog.Description = Guid.NewGuid().ToString();
                blog.Account = user2;
                Session.Save(blog);
                Session.Flush();
            }
            finally
            {
                Session = null;
                // delete temp user
                DeleteUser(user2);
                // delete blog
                CurrentUserContext = saved;
                Session.Delete(blog);
                Session.Flush();
            }
        }
    }
}
