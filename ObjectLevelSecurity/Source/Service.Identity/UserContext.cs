using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using Vestris.Data.NHibernate;
using Vestris.Service.NHibernate;

namespace Vestris.Service.Identity
{
    /// <summary>
    /// An authenticated user context.
    /// </summary>
    public class UserContext : EmptySessionContext
    {
        private int _accountId = 0;
        private DateTime _timestamp = DateTime.UtcNow;

        /// <summary>
        /// Authenticated account id.
        /// </summary>
        public int AccountId { get { return _accountId; } }
       
        /// <summary>
        /// Timestamp when the context was manufactured.
        /// </summary>
        public DateTime TimeStamp { get { return _timestamp; } }

        protected UserContext(ISession session)
            : base(session)
        {

        }

        public UserContext(ISession session, Account account)
            : base(session)
        {
            _accountId = account.Id;
        }
    }

    public class GuestUserContext : UserContext
    {
        public GuestUserContext(ISession session)
            : base(session)
        {

        }
    }
}
