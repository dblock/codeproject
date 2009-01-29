using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Data.NHibernate;
using System.Reflection;
using NHibernate;
using Vestris.Service.Identity;
using Vestris.Service.NHibernate;

namespace Vestris.Service.Data
{
    public class ServiceDataInterceptor : EmptyInterceptor
    {
        public override object Instantiate(Type clazz, object id)
        {
            Console.WriteLine("Instantiate: {0}:{1}", clazz, id);
            return base.Instantiate(clazz, id);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            Console.WriteLine("FlushDirty: {0}:{1} (ctx: {2})", entity, id, CurrentUserContext.AccountId);

            if (entity is IDataObject)
            {
                Check((IDataObject)entity, DataOperation.Update);
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        private UserContext CurrentUserContext
        {
            get
            {
                return (UserContext)SessionManager.CurrentSessionContext;
            }
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            Console.WriteLine("Save: {0}:{1} (ctx: {2})", entity, id, CurrentUserContext.AccountId);
            
            if (entity is IDataObject)
            {
                Check((IDataObject)entity, DataOperation.Create);
            }

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            Console.WriteLine("Load: {0}:{1} (ctx: {2})", entity, id, CurrentUserContext.AccountId);

            if (entity is IDataObject)
            {
                Check((IDataObject)entity, DataOperation.Retreive);
            }

            return base.OnLoad(entity, id, state, propertyNames, types);
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            Console.WriteLine("Delete: {0}:{1} (ctx: {2})", entity, id, CurrentUserContext.AccountId);

            if (entity is IDataObject)
            {
                Check((IDataObject)entity, DataOperation.Delete);
            }

            base.OnDelete(entity, id, state, propertyNames, types);
        }

        private void Check(IDataObject instance, DataOperation op)
        {
            // create an instance of an ACL
            string aclClassTypeName = string.Format("Vestris.Service.Data.{0}ClassACL", instance.GetType().Name);            
            Type aclClassType = Assembly.GetExecutingAssembly().GetType(aclClassTypeName, true, false);
            object[] args = { instance };
            ACL acl = (ACL) Activator.CreateInstance(aclClassType, args);
            acl.Check(CurrentUserContext, op);
        }
    }
}
