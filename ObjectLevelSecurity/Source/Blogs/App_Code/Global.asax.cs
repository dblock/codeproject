using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Diagnostics;
using NHibernate;
using NHibernate.Expression;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Hosting;
using Vestris.Service.NHibernate;
using Vestris.Service.Identity;
using Vestris.Service.Data;
using System.Web.Security;

public class Global : HttpApplication
{
    public void Application_Start(Object sender, EventArgs e)
    {
        SessionManager.Initialize(new HttpSessionSource(), new ServiceDataInterceptor());
    }

    public void Application_BeginRequest(Object sender, EventArgs e)
    {
        SessionManager.BeginRequest();
    }

    public void Application_EndRequest(Object sender, EventArgs e)
    {
        SessionManager.EndRequest();
    }

    public void Application_PostAuthenticateRequest(Object sender, EventArgs e)
    {
        IdentityServiceMembershipUser user = (IdentityServiceMembershipUser) Membership.GetUser();
        if (user == null || user.Account == null)
        {
            SessionManager.CurrentSessionContext = new GuestUserContext(
                SessionManager.Current);
        }
        else
        {
            SessionManager.CurrentSessionContext = new UserContext(
                SessionManager.Current, user.Account);
        }
    }
}
