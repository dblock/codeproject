using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Event;
using NHibernate.Cfg;
using NHibernate.Event.Default;

namespace Vestris.Service.NHibernate
{
    /// <summary>
    /// Session factory event listeners.
    /// </summary>
    public class SessionFactoryEventListeners
    {
        public IPostLoadEventListener PostLoadEventListener;
        public IPreDeleteEventListener PreDeleteEventListener;
        public IPreInsertEventListener PreInsertEventListener;
        public ISaveOrUpdateEventListener SaveOrUpdateEventListener;

        private T[] Insert<T>(T[] listeners, T instance)
        {
            if (instance == null)
                return listeners;

            List<T> newListeners = new List<T>();
            newListeners.Add(instance);
            newListeners.AddRange(listeners);
            return newListeners.ToArray();
        }

        public void Configure(Configuration cfg)
        {
            cfg.EventListeners.PostLoadEventListeners = Insert(cfg.EventListeners.PostLoadEventListeners, 
                PostLoadEventListener);
            cfg.EventListeners.PreDeleteEventListeners = Insert(cfg.EventListeners.PreDeleteEventListeners,
                PreDeleteEventListener);
            cfg.EventListeners.PreInsertEventListeners = Insert(cfg.EventListeners.PreInsertEventListeners, 
                PreInsertEventListener);
            cfg.EventListeners.SaveOrUpdateEventListeners = Insert(cfg.EventListeners.SaveOrUpdateEventListeners, 
                SaveOrUpdateEventListener);
        }
    }
}
