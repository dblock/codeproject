using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Vestris.Data.NHibernate.UnitTests
{
    [TestFixture]
    public class BlogUnitTests : NHibernateCrudTest<Blog>
    {
        private AccountUnitTests _account = new AccountUnitTests();

        public BlogUnitTests()
        {
            _instance.Name = Guid.NewGuid().ToString();
            _instance.Description = Guid.NewGuid().ToString();
            _instance.Account = _account.Instance;
            _instance.Created = DateTime.UtcNow;
            AddDependentObject(_account);
        }
    }
}
