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
            // allow the blog owner to do everything with the post
            this.Add(new ACLAccount(instance.Blog.Account, DataOperation.All));
            // allow the author of the post to do everything with the post
            this.Add(new ACLAccount(instance.Account, DataOperation.AllExceptCreate));
            // allow blog authors to create posts
            if (instance.Blog.BlogAuthors != null)
            {
                foreach (BlogAuthor author in instance.Blog.BlogAuthors)
                {
                    this.Add(new ACLAccount(author.Account, DataOperation.Create));
                }
            }
        }
    }
}
