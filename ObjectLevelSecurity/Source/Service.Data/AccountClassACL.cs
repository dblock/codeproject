using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Data.NHibernate;

namespace Vestris.Service.Data
{
    public class AccountClassACL : ACL
    {
        public AccountClassACL(Account instance)
        {
            // allow everyone to create an account
            this.Add(new ACLEveryoneAllowCreate());
            this.Add(new ACLAccount(instance, DataOperation.All));
        }
    }
}
