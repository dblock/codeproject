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

namespace DirtyPanel
{
    public partial class _Slave : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void save_Click(object sender, EventArgs e)
        {
            DirtyPanelExtender.DirtyPanelExtender extender = ((DirtyPanel.Master)Page.Master).DirtyPanelExtender;
            extender.ResetDirtyFlag();
            lastsaved.Text = DateTime.Now.ToString();
        }
    }
}
