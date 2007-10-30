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
using System.Collections.Generic;
using DBlock;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            linkDynamic.NavigateUrl = string.Format("Default.aspx?id={0}", 
                Guid.NewGuid());

            string id = Request["id"];
            if (!string.IsNullOrEmpty(id))
            {
                List<KeyValuePair<string, Uri>> nodes = new List<KeyValuePair<string, Uri>>();
                nodes.Add(new KeyValuePair<string, Uri>("Dynamic Content", new Uri(Request.Url, "Default.aspx?id=")));
                nodes.Add(new KeyValuePair<string, Uri>(Request["id"], Request.Url));
                ((SiteMapDataProvider) SiteMap.Provider).Stack(nodes);
            }
        }
    }
}
