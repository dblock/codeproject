using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Data.NHibernate;

namespace Vestris.Service.Data
{
    public class BlogAuthorClassACL : ACL
    {
        public BlogAuthorClassACL(BlogAuthor instance)
        {
            // allow the author himself to see his membership information
            this.Add(new ACLAccount(instance.Account, DataOperation.Retreive));
            // allow the blog owner to add/delete/view authors
            this.Add(new ACLAccount(instance.Blog.Account, DataOperation.All));
            // allow everyone to get information about this BlogAuthor
            this.Add(new ACLEveryoneAllowRetrieve());
        }
    }
}
