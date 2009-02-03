using System;
using System.Collections.Generic;
using System.Text;
using Vestris.Data.NHibernate;
using Vestris.Service.Identity;

namespace Vestris.Service.Data
{
    public enum ACLVerdict
    {
        None,
        Denied,
        Allowed
    };

    public interface IACLEntry
    {
        ACLVerdict Apply(UserContext ctx, DataOperation op);
    };

    public abstract class ACLBaseEntry : IACLEntry
    {
        protected int _operation = 0;
        protected DataOperationPermission _permission = DataOperationPermission.Deny;

        public ACLBaseEntry(DataOperation op)
            : this(op, DataOperationPermission.Allow)
        {

        }

        public ACLBaseEntry(DataOperation op, DataOperationPermission perm)
            : this((int) op, perm)
        {

        }

        public ACLBaseEntry(int op, DataOperationPermission perm)
        {
            _operation = op;
            _permission = perm;
        }

        public abstract ACLVerdict Apply(UserContext ctx, DataOperation op);
    }

    public class ACLEveryoneAllowCreate : ACLBaseEntry
    {
        public ACLEveryoneAllowCreate()
            : base(DataOperation.Create, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(UserContext ctx, DataOperation op)
        {
            return (op == DataOperation.Create) ? ACLVerdict.Allowed : ACLVerdict.None;
        }
    }

    public class ACLEveryoneAllowRetrieve : ACLBaseEntry
    {
        public ACLEveryoneAllowRetrieve()
            : base(DataOperation.Retreive, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(UserContext ctx, DataOperation op)
        {
            return (op == DataOperation.Retreive) ? ACLVerdict.Allowed : ACLVerdict.None;
        }
    }

    public class ACLAuthenticatedAllowRetrieve : ACLBaseEntry
    {
        public ACLAuthenticatedAllowRetrieve()
            : base(DataOperation.Retreive, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(UserContext ctx, DataOperation op)
        {
            if (op == DataOperation.Retreive && ctx.AccountId != 0)
            {
                return ACLVerdict.Allowed;
            }

            return ACLVerdict.None;
        }
    }

    public class ACLAuthenticatedAllowCreateAndDelete : ACLBaseEntry
    {
        public ACLAuthenticatedAllowCreateAndDelete()
            : base(DataOperation.Create | DataOperation.Delete, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(UserContext ctx, DataOperation op)
        {
            if ((op == DataOperation.Create || op == DataOperation.Delete) && ctx.AccountId != 0)
            {
                return ACLVerdict.Allowed;
            }

            return ACLVerdict.None;
        }
    }

    public class ACLAuthenticatedAllowCreate : ACLBaseEntry
    {
        public ACLAuthenticatedAllowCreate()
            : base(DataOperation.Create, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(UserContext ctx, DataOperation op)
        {
            if (op == DataOperation.Create && ctx.AccountId != 0)
            {
                return ACLVerdict.Allowed;
            }

            return ACLVerdict.None;
        }
    }

    public class ACLAccount : ACLBaseEntry
    {
        private Account _account = null;

        public ACLAccount(Account value, int op)
            : this(value, op, DataOperationPermission.Allow)
        {

        }

        public ACLAccount(Account value, DataOperation op)
            : this(value, (int)op, DataOperationPermission.Allow)
        {

        }

        public ACLAccount(Account value, int op, DataOperationPermission perm)
            : base(op, perm)
        {
            _account = value;
        }

        public override ACLVerdict Apply(UserContext ctx, DataOperation op)
        {
            if (ctx.AccountId == 0 || _account == null)
                return ACLVerdict.None;

            if (ctx.AccountId != _account.Id)
                return ACLVerdict.None;

            if ((_operation & (int)op) == 0)
                return ACLVerdict.None;

            return _permission == DataOperationPermission.Allow
                ? ACLVerdict.Allowed
                : ACLVerdict.Denied;
        }

        public static IList<IACLEntry> GetACLEntries(IList<Account> accounts, int op, DataOperationPermission perm)
        {
            List<IACLEntry> result = new List<IACLEntry>();
            foreach (Account account in accounts)
            {
                result.Add(new ACLAccount(account, op, perm));
            }
            return result;
        }
    }

    public class ACLAccountId : ACLBaseEntry
    {
        private int _accountId = 0;

        public ACLAccountId(int value, int op)
            : this(value, op, DataOperationPermission.Allow)
        {

        }

        public ACLAccountId(int value, DataOperation op)
            : this(value, (int)op, DataOperationPermission.Allow)
        {

        }

        public ACLAccountId(int value, int op, DataOperationPermission perm)
            : base(op, perm)
        {
            _accountId = value;
        }

        public override ACLVerdict Apply(UserContext ctx, DataOperation op)
        {
            if (ctx.AccountId == 0)
                return ACLVerdict.None;

            if (ctx.AccountId != _accountId)
                return ACLVerdict.None;

            if ((_operation & (int)op) == 0)
                return ACLVerdict.None;

            return _permission == DataOperationPermission.Allow
                ? ACLVerdict.Allowed
                : ACLVerdict.Denied;
        }

        public static IList<IACLEntry> GetACLEntries(IList<Account> accounts, int op, DataOperationPermission perm)
        {
            List<IACLEntry> result = new List<IACLEntry>();
            foreach (Account account in accounts)
            {
                result.Add(new ACLAccountId(account.Id, op, perm));
            }
            return result;
        }
    }

    public class ACL
    {
        private List<IACLEntry> _accessControlList = new List<IACLEntry>();

        public List<IACLEntry> AccessControlList
        {
            get
            {
                return _accessControlList;
            }
        }

        public ACL()
        {

        }

        public ACL(ACL value)
        {
            _accessControlList.AddRange(value._accessControlList);
        }

        public ACLVerdict Apply(UserContext ctx, DataOperation op)
        {
            ACLVerdict current = ACLVerdict.Denied;

            foreach (IACLEntry entry in _accessControlList)
            {
                ACLVerdict result = entry.Apply(ctx, op);
                switch (result)
                {
                    case ACLVerdict.Denied:
                        return ACLVerdict.Denied;
                    case ACLVerdict.Allowed:
                        current = ACLVerdict.Allowed;
                        break;
                }
            }

            return current;
        }

        public bool TryCheck(UserContext ctx, DataOperation op)
        {
            ACLVerdict result = Apply(ctx, op);
            switch (result)
            {
                case ACLVerdict.Denied:
                case ACLVerdict.None:
                    return false;
            }

            return true;
        }

        public void Check(UserContext ctx, DataOperation op)
        {
            if (!TryCheck(ctx, op))
            {
                throw new AccessDeniedException();
            }
        }

        public void Add(ACL value)
        {
            _accessControlList.AddRange(value.AccessControlList);
        }

        public void Add(IACLEntry value)
        {
            _accessControlList.Add(value);
        }

        public void AddRange(IList<IACLEntry> collection)
        {
            _accessControlList.AddRange(collection);
        }

        public int Count
        {
            get
            {
                return _accessControlList.Count;
            }
        }
    }
}
