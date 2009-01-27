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
            // everyone can see accounts
            this.Add(new ACLEveryoneAllowRetrieve());
            // owner can do everything with his own account
            this.Add(new ACLAccount(instance, DataOperation.All));
        }
    }
}
