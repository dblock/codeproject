using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Data.NHibernate;
using NHibernate;
using NHibernate.Expression;

namespace Vestris.Service.Identity
{
    public class IdentityService
    {
        private ISession _session = null;

        public IdentityService(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Login a user.
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">user password</param>
        /// <returns>a user security context</returns>
        public UserContext Login(string username, string password)
        {
            // find an account by username/passsword
            Account account = _session.CreateCriteria(typeof(Account))
                .Add(Expression.Eq("Name", username))
                .Add(Expression.Eq("Password", password))
                .UniqueResult<Account>();

            if (account == null)
            {
                throw new AccessDeniedException();
            }

            return new UserContext(_session, account);
        }
    }
}
