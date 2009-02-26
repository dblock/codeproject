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
        Page.RegisterHiddenField("__EVENTTARGET", "ctl00$ContentPlaceHolder1$createBlogPost");

        if (!IsPostBack)
        {
            Blog blog = SessionManager.CurrentSession.Load<Blog>(
                Int32.Parse(Request["id"]));

            blogName.Text = Title = blog.Name;

            GetAccounts();
            GetBlogPosts();
            GetBlogAuthors();
        }
    }

    public void linkDeleteBlogPost_Command(object sender, CommandEventArgs e)
    {
        int id = int.Parse(e.CommandArgument.ToString());
        BlogPost blogPost = SessionManager.CurrentSession.Load<BlogPost>(id);
        SessionManager.CurrentSession.Delete(blogPost);
        SessionManager.CurrentSession.Flush();
        GetBlogPosts();
    }

    public void linkDeleteBlogAuthor_Command(object sender, CommandEventArgs e)
    {
        int id = int.Parse(e.CommandArgument.ToString());
        BlogAuthor blogAuthor = SessionManager.CurrentSession.Load<BlogAuthor>(id);
        SessionManager.CurrentSession.Delete(blogAuthor);
        SessionManager.CurrentSession.Flush();
        GetBlogAuthors();
    }

    private void GetAccounts()
    {
        // todo: only list those that aren't already authors or blog owner
        listAccounts.DataSource = SessionManager.CurrentSession.CreateCriteria(typeof(Account))
            .List<Account>();
        listAccounts.DataBind();
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

    private void GetBlogAuthors()
    {
        Blog blog = SessionManager.CurrentSession.Load<Blog>(
            Int32.Parse(Request["id"]));
        gridBlogAuthors.DataSource = SessionManager.CurrentSession.CreateCriteria(typeof(BlogAuthor))
            .Add(Expression.Eq("Blog", blog))
            .List<BlogAuthor>();
        gridBlogAuthors.DataBind();
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

    public void createBlogAuthor_Click(object sender, EventArgs e)
    {
        IdentityServiceMembershipUser user = (IdentityServiceMembershipUser)Membership.GetUser();
        Blog blog = SessionManager.CurrentSession.Load<Blog>(Int32.Parse(Request["id"]));
        BlogAuthor author = new BlogAuthor();
        author.Account = SessionManager.CurrentSession.Load<Account>(Int32.Parse(listAccounts.SelectedValue));
        author.Blog = blog;
        SessionManager.CurrentSession.Save(author);
        SessionManager.CurrentSession.Flush();
        GetBlogAuthors();
    }
}
