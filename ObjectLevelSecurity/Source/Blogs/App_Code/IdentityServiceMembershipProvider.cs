using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Vestris.Service.NHibernate;
using Vestris.Service.Identity;

public class IdentityServiceMembershipProvider : MembershipProvider
{
    private SessionFactory _sessionFactory = new SessionFactory(null);
    private string _applicationName = "IdentityService";

    public IdentityServiceMembershipProvider()
    {
    }

    public override bool ValidateUser(string username, string password)
    {
        IdentityService identityService = new IdentityService(_sessionFactory.Instance.OpenSession());
        return identityService.TryLogin(username, password);
    }

    public override bool EnablePasswordRetrieval
    {
        get { return false; }
    }

    public override bool EnablePasswordReset
    {
        get { return false; }
    }

    public override bool RequiresQuestionAndAnswer
    {
        get { return false; }
    }

    public override int MaxInvalidPasswordAttempts
    {
        get { return 3; }
    }

    public override bool RequiresUniqueEmail
    {
        get { return false; }
    }


    public override string ApplicationName
    {
        get
        {
            return _applicationName;
        }
        set
        {
            _applicationName = value;
        }
    }

    public override int PasswordAttemptWindow
    {
        get { return 10; }
    }

    public override MembershipPasswordFormat PasswordFormat
    {
        get { return MembershipPasswordFormat.Clear; }
    }

    public override int MinRequiredNonAlphanumericCharacters
    {
        get { return 0; }
    }

    public override int MinRequiredPasswordLength
    {
        get { return 6; }
    }

    public override string PasswordStrengthRegularExpression
    {
        get { return string.Empty; }
    }

    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
    {
        throw new NotImplementedException();
    }

    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
    {
        IdentityService identityService = new IdentityService(_sessionFactory.Instance.OpenSession());
        MembershipUser user = new IdentityServiceMembershipUser(identityService.CreateUser(username, password));
        status = MembershipCreateStatus.Success;
        return user;
    }

    protected override byte[] DecryptPassword(byte[] encodedPassword)
    {
        return base.DecryptPassword(encodedPassword);
    }

    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    {
        throw new NotImplementedException();
    }

    public override string Description
    {
        get
        {
            return base.Description;
        }
    }

    protected override byte[] EncryptPassword(byte[] password)
    {
        return base.EncryptPassword(password);
    }

    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
    {
        throw new NotImplementedException();
    }

    public override int GetNumberOfUsersOnline()
    {
        throw new NotImplementedException();
    }

    public override string GetPassword(string username, string answer)
    {
        throw new NotImplementedException();
    }

    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    {
        IdentityService identityService = new IdentityService(_sessionFactory.Instance.OpenSession());
        return new IdentityServiceMembershipUser(identityService.GetUserById((int) providerUserKey));
    }

    public override MembershipUser GetUser(string username, bool userIsOnline)
    {
        IdentityService identityService = new IdentityService(_sessionFactory.Instance.OpenSession());
        return new IdentityServiceMembershipUser(identityService.FindUser(username));
    }

    public override string GetUserNameByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public override string ResetPassword(string username, string answer)
    {
        throw new NotImplementedException();
    }

    public override bool UnlockUser(string userName)
    {
        throw new NotImplementedException();
    }

    public override void UpdateUser(MembershipUser user)
    {
        throw new NotImplementedException();
    }
}
