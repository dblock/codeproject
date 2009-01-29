using System;
using System.Collections.Generic;
using System.Text;

namespace Vestris.Service.NHibernate
{
    public class SessionManagerContextPusher : IDisposable
    {
        private ISessionContext _previousCtx = null;

        public SessionManagerContextPusher(ISessionContext newContext)
        {
            _previousCtx = SessionManager.CurrentSessionContext;
            SessionManager.CurrentSessionContext = newContext;
        }

        public void Dispose()
        {
            if (_previousCtx != null)
            {
                SessionManager.CurrentSessionContext = _previousCtx;
                _previousCtx = null;
            }
        }
    }
}
