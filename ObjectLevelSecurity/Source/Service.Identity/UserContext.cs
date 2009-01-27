using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Data.NHibernate;

namespace Vestris.Service.Identity
{
    /// <summary>
    /// An authenticated user context.
    /// </summary>
    public class UserContext
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

        protected UserContext()
        {
        }

        public UserContext(Account account)
        {
            _accountId = account.Id;
        }
    }

    public class GuestUserContext : UserContext
    {
        public GuestUserContext()
        {

        }
    }
}
