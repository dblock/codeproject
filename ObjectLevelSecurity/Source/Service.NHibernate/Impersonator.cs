using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace Vestris.Service.NHibernate
{
    public class Impersonator : IDisposable
    {
        private ISessionContext _previousCtx = null;
        private ISession _previousSession = null;

        public Impersonator(ISessionContext newContext)
        {
            _previousCtx = SessionManager.CurrentSessionContext;
            _previousSession = SessionManager.CurrentSession;
            SessionManager.CurrentSessionContext = newContext;
            SessionManager.CurrentSession = SessionManager.Open();
        }

        public void Dispose()
        {
            if (_previousCtx != null)
            {
                SessionManager.CurrentSessionContext = _previousCtx;
                _previousCtx = null;
            }

            if (_previousSession != null)
            {
                SessionManager.CurrentSession = _previousSession;
                _previousSession = null;
            }
        }
    }
}
