using System;
using System.Collections.Generic;
using System.Text;

namespace Vestris.Service.Data
{
    public enum DataOperation
    {
        None = 0,
        Create = 1,
        Retreive = 2,
        Update = 4,
        Delete = 8,
        All = Create | Retreive | Update | Delete,
        AllExceptCreate = Retreive | Update | Delete,
        AllExceptUpdate = Create | Retreive | Delete,
        AllExceptDelete = Create | Retreive | Update
    };

    public enum DataOperationPermission
    {
        Deny,
        Allow,
    };
}
