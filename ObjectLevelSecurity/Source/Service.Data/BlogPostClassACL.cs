using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Data.NHibernate;

namespace Vestris.Service.Data
{
    public class BlogPostClassACL : ACL
    {
        public BlogPostClassACL(BlogPost instance)
        {
            // allow the author of the post to do everything with the post
            this.Add(new ACLAccount(instance.Account, DataOperation.AllExceptCreate));
            // allow blog authors to create posts
            foreach (BlogAuthor author in instance.Blog.BlogAuthors)
            {
                this.Add(new ACLAccount(author.Account, DataOperation.Create));
            }
            // allow the blog owner to add/delete/view posts
            this.Add(new ACLAccount(instance.Blog.Account, DataOperation.All));
            // allow everyone to get information about this blog post
            this.Add(new ACLEveryoneAllowRetrieve());
        }
    }
}
