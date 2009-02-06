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

public partial class BlogPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Blog blog = SessionManager.Current.Load<Blog>(
            Int32.Parse(Request["id"]));
        
        Title = blog.Name;
    }
}
