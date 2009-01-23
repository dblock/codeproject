using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Vestris.Data.NHibernate.UnitTests
{
    [TestFixture]
    public class AccountUnitTests : NHibernateCrudTest<Account>
    {
        public AccountUnitTests()
        {
            _instance.Name = Guid.NewGuid().ToString();
            _instance.Password = "password";
            _instance.Created = DateTime.UtcNow;
        }
    }
}
