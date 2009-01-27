using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Data.NHibernate;
using System.Reflection;
using NHibernate;
using Vestris.Service.Identity;

namespace Vestris.Service.Data
{
    public class ServiceDataInterceptor : EmptyInterceptor
    {
        private UserContext _ctx = null;

        public UserContext UserContext
        {
            get
            {
                return _ctx;
            }
            set
            {
                _ctx = value;
            }
        }

        public ServiceDataInterceptor(UserContext ctx)
        {
            _ctx = ctx;
        }

        public override object Instantiate(Type clazz, object id)
        {
            Console.WriteLine("Instantiate: {0}:{1}", clazz, id);
            return base.Instantiate(clazz, id);
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            Console.WriteLine("Save: {0}:{1}", entity, id);
            
            if (entity is IDataObject)
            {
                Check((IDataObject)entity, id == null ? DataOperation.Create : DataOperation.Update);
            }

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            Console.WriteLine("Load: {0}:{1}", entity, id);

            if (entity is IDataObject)
            {
                Check((IDataObject)entity, DataOperation.Retreive);
            }

            return base.OnLoad(entity, id, state, propertyNames, types);
        }

        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            Console.WriteLine("Delete: {0}:{1}", entity, id);

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
            acl.Check(_ctx, op);
        }
    }
}
