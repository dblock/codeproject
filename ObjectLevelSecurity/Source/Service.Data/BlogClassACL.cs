using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Data.NHibernate;

namespace Vestris.Service.Data
{
    public class BlogClassACL : ACL
    {
        public BlogClassACL(Blog instance)
        {
            // allow every authenticated user to create a blog
            this.Add(new ACLAuthenticatedAllowCreate());
            // allow everyone to get information about this blog
            this.Add(new ACLEveryoneAllowRetrieve());
            // the owner has full privileges
            this.Add(new ACLAccount(instance.Account, DataOperation.All));
        }
    }
}
