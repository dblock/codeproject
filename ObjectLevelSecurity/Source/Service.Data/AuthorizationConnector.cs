using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vestris.Data.NHibernate;
using Vestris.Service.Identity;
using Vestris.Service.NHibernate;
using System.Reflection;

namespace Vestris.Service.Data
{
    public abstract class ServiceDataAuthorizationConnector
    {
        /// <summary>
        /// Manufacture and check object ACL against the current session context.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="op"></param>
        public static void Check(IDataObject instance, DataOperation op)
        {
            // create an instance of an ACL
            string aclClassTypeName = string.Format("Vestris.Service.Data.{0}ClassACL", instance.GetType().Name);
            Type aclClassType = Assembly.GetExecutingAssembly().GetType(aclClassTypeName, true, false);
            object[] args = { instance };
            ACL acl = (ACL)Activator.CreateInstance(aclClassType, args);
            acl.Check((UserContext) SessionManager.CurrentSessionContext, op);
        }
    }
}
