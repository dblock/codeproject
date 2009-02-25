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
using NHibernate.Criterion;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.RegisterHiddenField("__EVENTTARGET", "ctl00$ContentPlaceHolder1$createBlog");
        IdentityServiceMembershipUser user = (IdentityServiceMembershipUser) Membership.GetUser();
        labelBlogs.Text = string.Format("{0}'s Blogs", user.UserName);
        GetBlogs();
    }

    public void linkDelete_Command(object sender, CommandEventArgs e)
    {
        int id = int.Parse(e.CommandArgument.ToString());
        Blog blog = SessionManager.CurrentSession.Load<Blog>(id);
        SessionManager.CurrentSession.Delete(blog);
        SessionManager.CurrentSession.Flush();
        GetBlogs();
    }

    private void GetBlogs()
    {
        IdentityServiceMembershipUser user = (IdentityServiceMembershipUser)Membership.GetUser();
        // get the user's blogs
        gridBlogs.DataSource = SessionManager.CurrentSession.CreateCriteria(typeof(Blog))
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
        SessionManager.CurrentSession.Save(blog);
        SessionManager.CurrentSession.Flush();
        GetBlogs();
    }
}
