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
    public partial class _Simple : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void save_Click(object sender, EventArgs e)
        {
            demoPanelExtender.ResetDirtyFlag();
            lastsaved.Text = DateTime.Now.ToString();
        }

        public void demoListBoxAdd_Click(object sender, EventArgs e)
        {
            demoListBox.Items.Add(new ListItem(Guid.NewGuid().ToString()));
        }

        public void demoListBoxMultipleAdd_Click(object sender, EventArgs e)
        {
            demoListBoxMultiple.Items.Add(new ListItem(Guid.NewGuid().ToString()));
        }

        public void demoDropDownAdd_Click(object sender, EventArgs e)
        {
            demoDropDown.Items.Add(new ListItem(Guid.NewGuid().ToString()));
        }

        public void demoRadioAdd_Click(object sender, EventArgs e)
        {
            demoRadio.Items.Add(new ListItem(Guid.NewGuid().ToString()));
        }
    }
}
