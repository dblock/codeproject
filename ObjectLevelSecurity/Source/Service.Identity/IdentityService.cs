using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;

namespace Vestris.Service.Identity
{
    public class IdentityService
    {
        private ISession _session = null;

        public IdentityService(ISession session)
        {
            _session = session;
        }

        public bool TryLogin(string username, string password)
        {
            UserContext ctx = null;
            return TryLogin(username, password, out ctx);
        }

        /// <summary>
        /// Try to login a user.
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">user password</param>
        /// <param name="ctx">user context</param>
        /// <returns>a user security context</returns>
        public bool TryLogin(string username, string password, out UserContext ctx)
        {
            ctx = null;

            // find an account by username/passsword
            Account account = _session.CreateCriteria(typeof(Account))
                .Add(Expression.Eq("Name", username))
                .Add(Expression.Eq("Password", password))
                .UniqueResult<Account>();

            if (account != null)
            {
                ctx = new UserContext(account);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        public Account CreateUser(string username, string password)
        {
            Account account = new Account();
            account.Created = DateTime.UtcNow;
            account.Name = username;
            account.Password = password;
            _session.Save(account);
            _session.Flush();
            return account;
        }

        /// <summary>
        /// Find a user.
        /// </summary>
        /// <param name="username">unique username</param>
        /// <returns></returns>
        public Account FindUser(string username)
        {
            return _session.CreateCriteria(typeof(Account))
                .Add(Expression.Eq("Name", username))
                .UniqueResult<Account>();
        }

        /// <summary>
        /// Get a user by id.
        /// </summary>
        /// <returns></returns>
        public Account GetUserById(int id)
        {
            return _session.Load<Account>(id);
        }

        /// <summary>
        /// Login a user.
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">user password</param>
        /// <returns>a user security context</returns>
        public UserContext Login(string username, string password)
        {
            UserContext ctx = null;
            
            if (! TryLogin(username, password, out ctx))
                throw new AccessDeniedException();

            return ctx;
        }
    }
}
