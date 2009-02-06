using System;
using System.Data;
using NHibernate;

namespace Vestris.Service.NHibernate
{
    /// <summary>
    /// Reconnects NHibernate session to the database. Disconnects it while disposing.
    /// From Andrew Mayorov, http://wiki.nhibernate.org/display/NH/Using+NHibernate+with+ASP.Net
    /// </summary>
    public class SessionOpener : IDisposable
    {
        private bool _wasOpened = false;
        private bool _disposed = false;
        private ISession _session;
        private IDbConnection _connection;

        public SessionOpener(ISession session, IDbConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                _connection = connection;
                connection.Open();
                _wasOpened = true;
            }
            _session = session;
            session.Reconnect(connection);
        }

        ~SessionOpener()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                if (_session != null && _session.IsConnected)
                    _session.Disconnect();
                if (_wasOpened)
                    _connection.Close();
            }
            finally
            {
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}
