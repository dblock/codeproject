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
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            Console.WriteLine("FlushDirty: {0}:{1} (ctx: {2})", entity, id, CurrentUserContext.AccountId);

            if (entity is IDataObject)
            {
                ServiceDataAuthorizationConnector.Check((IDataObject)entity, DataOperation.Update);
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            Console.WriteLine("Save: {0}:{1} (ctx: {2})", entity, id, CurrentUserContext.AccountId);
            
            if (entity is IDataObject)
            {
                ServiceDataAuthorizationConnector.Check((IDataObject)entity, DataOperation.Create);
            }

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            Console.WriteLine("Load: {0}:{1} (ctx: {2})", entity, id, CurrentUserContext.AccountId);

            if (entity is IDataObject)
            {
                ServiceDataAuthorizationConnector.Check((IDataObject)entity, DataOperation.Retreive);
            }

            return base.OnLoad(entity, id, state, propertyNames, types);
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, global::NHibernate.Type.IType[] types)
        {
            Console.WriteLine("Delete: {0}:{1} (ctx: {2})", entity, id, CurrentUserContext.AccountId);

            if (entity is IDataObject)
            {
                ServiceDataAuthorizationConnector.Check((IDataObject)entity, DataOperation.Delete);
            }

            base.OnDelete(entity, id, state, propertyNames, types);
        }


        private UserContext CurrentUserContext
        {
            get
            {
                return (UserContext)SessionManager.CurrentSessionContext;
            }
        }
    }
}
