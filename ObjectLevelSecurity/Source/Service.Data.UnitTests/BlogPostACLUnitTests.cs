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
    public class BlogPostACLUnitTests : UserContextUnitTests
    {
        private Blog _blog = null;

        public Blog CreateBlog()
        {
            Blog instance = new Blog();
            instance.Account = _user;
            instance.Created = DateTime.UtcNow;
            instance.Description = Guid.NewGuid().ToString();
            instance.Name = Guid.NewGuid().ToString();
            Session.Save(instance);
            Session.Flush();
            return instance;
        }

        public void DeleteBlog(Blog instance)
        {
            using (new Impersonator(new UserContext(instance.Account)))
            {
                Blog instanceCopy = Session.Load<Blog>(instance.Id);
                Session.Delete(instanceCopy);
                Session.Flush();
            }
        }

        public override void SetUp()
        {
            base.SetUp();
            _blog = CreateBlog();
        }

        public override void TearDown()
        {
            DeleteBlog(_blog);
            base.TearDown();
        }

        [Test]
        public void TestCreateDelete()
        {
            // current user, also blog owner can create posts
            BlogPost post = new BlogPost();
            post.Account = _user;
            post.Blog = _blog;
            post.Title = Guid.NewGuid().ToString();
            post.Body = Guid.NewGuid().ToString();
            post.Created = DateTime.UtcNow;
            Session.Save(post);
            Session.Flush();
            Session.Delete(post);
            Session.Flush();
        }

        [Test, ExpectedException(typeof(AccessDeniedException))]
        public void TestCreateRetrieve()
        {
            // current user, also blog owner can create posts
            BlogPost post = new BlogPost();
            post.Account = _user;
            post.Blog = _blog;
            post.Title = Guid.NewGuid().ToString();
            post.Body = Guid.NewGuid().ToString();
            post.Created = DateTime.UtcNow;
            Session.Save(post);
            Session.Flush();

            try
            {
                Account user2 = CreateUser();
                // another user cannot read posts, he's not a blog author
                using (new Impersonator(new UserContext(user2)))
                {
                    BlogPost postCopy = Session.Load<BlogPost>(post.Id);
                    // if you don't resolve a field an object proxy is loaded
                    Console.WriteLine("Post: {0}", postCopy.Body);
                    Session.Flush();
                }
            }
            catch (ADOException ex)
            {
                throw ex.InnerException;
            }
            finally
            {
                Session.Delete(post);
                Session.Flush();
            }
        }

        [Test, ExpectedException(typeof(AccessDeniedException))]
        public void TestCreateAccessDenied()
        {
            Account user2 = CreateUser();
            using (new Impersonator(new UserContext(user2)))
            {
                try
                {
                    BlogPost post = new BlogPost();
                    // another user cannot post
                    post.Account = user2;
                    post.Blog = _blog;
                    post.Title = Guid.NewGuid().ToString();
                    post.Body = Guid.NewGuid().ToString();
                    post.Created = DateTime.UtcNow;
                    Session.Save(post);
                    Session.Flush();
                }
                finally
                {
                    DeleteUser(user2);
                }
            }
        }
    }
}
