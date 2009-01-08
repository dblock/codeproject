using System;
using System.Collections.Generic;
using System.Text;
using ActiveDs;
using System.DirectoryServices;
using Microsoft.CommandLine;
using System.Threading;

namespace RotatePassword
{
    class Program
    {
        class Args
        {
            [Argument(ArgumentType.Required, HelpText = "Current password.")]
            public string currentpassword = string.Empty;
            [Argument(ArgumentType.AtMostOnce, HelpText = "New password.")]
            public string newpassword = string.Empty;
            [Argument(ArgumentType.AtMostOnce, DefaultValue = 1, HelpText = "Number of times to rotate the password.")]
            public int times = 1;
        }

        static void Main(string[] args)
        {
            try
            {
                Args parsedArgs = new Args();
                if (Parser.ParseArgumentsWithUsage(args, parsedArgs))
                {
                    if (parsedArgs.times < 1) throw new ArgumentOutOfRangeException("times");
                    if (string.IsNullOrEmpty(parsedArgs.newpassword)) parsedArgs.newpassword = parsedArgs.currentpassword;

                    IADsADSystemInfo sysInfo = new ADSystemInfoClass();
                    Console.WriteLine("Current user: {0}", sysInfo.UserName);
                    DirectoryEntry currentUser = new DirectoryEntry(string.Format("LDAP://{0}", sysInfo.UserName));

                    string currentPassword = parsedArgs.currentpassword;
                    for (int i = 1; i <= parsedArgs.times; i++)
                    {
                        string newPassword = (i == parsedArgs.times) 
                            ? parsedArgs.newpassword 
                            : RandomPassword.Generate();

                        object[] passwordChangeRequest = new object[] 
                        {
                            currentPassword,
                            newPassword
                        };

                        Console.Write("{0} => {1}", currentPassword, newPassword); 
                        currentUser.Invoke("ChangePassword", passwordChangeRequest);
                        currentUser.CommitChanges();
                        currentPassword = newPassword;
                        Console.WriteLine(" [OK]"); 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null) Console.WriteLine(ex.InnerException.Message);
            }
        }
    }
}
