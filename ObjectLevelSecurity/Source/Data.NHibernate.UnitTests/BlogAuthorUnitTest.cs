using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Vestris.Data.NHibernate.UnitTests
{
    [TestFixture]
    public class BlogAuthorUnitTests : NHibernateCrudTest<BlogAuthor>
    {
        private BlogUnitTests _blog = new BlogUnitTests();
        private AccountUnitTests _author = new AccountUnitTests();

        public BlogAuthorUnitTests()
        {
            _instance.Account = _author.Instance;
            _instance.Blog = _blog.Instance;
            AddDependentObject(_blog);
            AddDependentObject(_author);
        }
    }
}
