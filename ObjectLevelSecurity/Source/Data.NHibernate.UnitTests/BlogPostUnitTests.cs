using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Vestris.Data.NHibernate.UnitTests
{
    [TestFixture]
    public class BlogPostUnitTests : NHibernateCrudTest<BlogPost>
    {
        private BlogUnitTests _blog = new BlogUnitTests();
        private AccountUnitTests _author = new AccountUnitTests();

        public BlogPostUnitTests()
        {
            _instance.Account = _author.Instance;
            _instance.Blog = _blog.Instance;
            _instance.Title = Guid.NewGuid().ToString();
            _instance.Body = Guid.NewGuid().ToString();
            _instance.Created = DateTime.UtcNow;
            AddDependentObject(_blog);
            AddDependentObject(_author);
        }
    }
}
