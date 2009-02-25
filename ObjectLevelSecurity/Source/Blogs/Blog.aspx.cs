using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Vestris.Data.NHibernate;
using Vestris.Service.NHibernate;
using NHibernate.Criterion;

public partial class BlogPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Blog blog = SessionManager.CurrentSession.Load<Blog>(
            Int32.Parse(Request["id"]));

        blogName.Text = Title = blog.Name;

        GetBlogPosts();
    }

    private void GetBlogPosts()
    {
        Blog blog = SessionManager.CurrentSession.Load<Blog>(
            Int32.Parse(Request["id"]));
        gridBlogPosts.DataSource = SessionManager.CurrentSession.CreateCriteria(typeof(BlogPost))
            .Add(Expression.Eq("Blog", blog))
            .List<BlogPost>();
        gridBlogPosts.DataBind();
    }

    public void createBlogPost_Click(object sender, EventArgs e)
    {
        IdentityServiceMembershipUser user = (IdentityServiceMembershipUser)Membership.GetUser();
        Blog blog = SessionManager.CurrentSession.Load<Blog>(Int32.Parse(Request["id"]));
        BlogPost post = new BlogPost();
        post.Account = user.Account;
        post.Blog = blog;
        post.Created = DateTime.UtcNow;
        post.Title = inputBlogPostTitle.Text;
        post.Body = inputBlogPostBody.Text;
        SessionManager.CurrentSession.Save(post);
        SessionManager.CurrentSession.Flush();
        GetBlogPosts();
    }
}
