using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NHibernate;

namespace Vestris.Service.NHibernate
{
    public interface ISessionStorage
    {
        ISession Session { get; set; }
        ISessionContext Context { get; set; }
    }

    /// <summary>
    /// Stores a session in the <see cref="HttpContext.Items" /> collection.
    /// </summary>
    public class HttpSessionSource : ISessionStorage
    {
        public ISessionContext Context
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                return (ISessionContext)HttpContext.Current.Items["NHibernate.ISessionContext"];
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items["NHibernate.ISessionContext"] = value;
                }
            }
        }

        public ISession Session
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                return (ISession)HttpContext.Current.Items["NHibernate.ISession"];
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items["NHibernate.ISession"] = value;
                }
            }
        }
    }

    /// <summary>
    /// Stores a session in the thread-static class member.
    /// </summary>
    public class ThreadSessionSource : ISessionStorage
    {
        [ThreadStatic]
        private static ISession _session = null;
        [ThreadStatic]
        private static ISessionContext _sessionContext = null;

        public ISession Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        public ISessionContext Context
        {
            get
            {
                return _sessionContext;
            }
            set
            {
                _sessionContext = value;
            }
        }
    }
}
