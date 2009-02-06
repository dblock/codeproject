using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Vestris.Service.NHibernate;
using Vestris.Data.NHibernate;
using NHibernate.Expression;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // get the current user
        IdentityServiceMembershipUser user = (IdentityServiceMembershipUser) Membership.GetUser();
        labelUserContext.Text = user.UserName;
        GetBlogs();
    }

    private void GetBlogs()
    {
        IdentityServiceMembershipUser user = (IdentityServiceMembershipUser)Membership.GetUser();
        // get the user's blogs
        gridBlogs.DataSource = SessionManager.Current.CreateCriteria(typeof(Blog))
            .Add(Expression.Eq("Account", user.Account))
            .List<Blog>();
        gridBlogs.DataBind();
    }

    public void createBlog_Click(object sender, EventArgs e)
    {
        IdentityServiceMembershipUser user = (IdentityServiceMembershipUser)Membership.GetUser();
        Blog blog = new Blog();
        blog.Account = user.Account;
        blog.Created = DateTime.UtcNow;
        blog.Name = inputBlogName.Text;
        SessionManager.Current.Save(blog);
        SessionManager.Current.Flush();
        GetBlogs();
    }
}
