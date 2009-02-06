using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Vestris.Data.NHibernate;
using Vestris.Service.Identity;

public class IdentityServiceMembershipUser : MembershipUser
{
    private Account _account = null;

    public IdentityServiceMembershipUser(Account account)
    {
        _account = account;
    }

    public Account Account
    {
        get
        {
            return _account;
        }
    }

    public override DateTime CreationDate
    {
        get
        {
            return _account.Created;
        }
    }

    public override string UserName
    {
        get
        {
            return _account.Name;
        }
    }
}
