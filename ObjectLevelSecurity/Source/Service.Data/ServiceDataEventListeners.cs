using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Event;
using Vestris.Data.NHibernate;
using Vestris.Service.NHibernate;
using System.Diagnostics;

namespace Vestris.Service.Data
{
    public abstract class ServiceDataEventListeners
    {
        private static SessionFactoryEventListeners _instance;

        static ServiceDataEventListeners()
        {
            _instance = new SessionFactoryEventListeners();
            _instance.PostLoadEventListener = new ServiceDataPostLoadEventListener();
            _instance.PreDeleteEventListener = new ServiceDataPreDeleteEventListner();
            _instance.PreInsertEventListener = new ServiceDataPreInsertEventListner();
            _instance.SaveOrUpdateEventListener = new ServiceDataSaveOrUpdateEventListener();
        }

        public static SessionFactoryEventListeners Instance
        {
            get
            {
                return _instance;
            }
            
        }
    }
    
    public class ServiceDataPostLoadEventListener : IPostLoadEventListener
    {
        #region ILoadEventListener Members

        public void OnPostLoad(PostLoadEvent @event)
        {
            Debug.WriteLine(string.Format("OnPostLoad - {0}", @event.Entity));
            if (@event.Entity is IDataObject)
            {
                ServiceDataAuthorizationConnector.Check((IDataObject) @event.Entity, 
                    DataOperation.Retreive);
            }
        }

        #endregion
    }

    public class ServiceDataPreDeleteEventListner : IPreDeleteEventListener
    {
        #region IPreDeleteEventListener Members

        public bool OnPreDelete(PreDeleteEvent @event)
        {
            Debug.WriteLine(string.Format("OnPreDelete - {0}", @event.Entity));

            if (@event.Entity is IDataObject)
            {
                // the function returns true if the operation should be vetoed
                // consider returning false instead of throwing
                ServiceDataAuthorizationConnector.Check((IDataObject)@event.Entity,
                    DataOperation.Delete);
            }

            return false;
        }

        #endregion
    }

    public class ServiceDataPreInsertEventListner : IPreInsertEventListener
    {
        #region IPreInsertEventListener Members

        public bool OnPreInsert(PreInsertEvent @event)
        {
            Debug.WriteLine(string.Format("OnPreInsert - {0}", @event.Entity));
            
            if (@event.Entity is IDataObject)
            {
                // the function returns true if the operation should be vetoed
                // consider returning false instead of throwing
                ServiceDataAuthorizationConnector.Check((IDataObject)@event.Entity,
                    DataOperation.Create);
            }

            return false;
        }

        #endregion
    }

    public class ServiceDataSaveOrUpdateEventListener : ISaveOrUpdateEventListener
    {
        #region ISaveOrUpdateEventListener Members

        public void OnSaveOrUpdate(SaveOrUpdateEvent @event)
        {
            Debug.WriteLine(string.Format("OnSaveOrUpdate - {0}", @event.Entity));

            if (@event.Entity is IDataObject)
            {
                ServiceDataAuthorizationConnector.Check((IDataObject)@event.Entity,
                    DataOperation.Retreive);
            }
        }

        #endregion
    }
}
