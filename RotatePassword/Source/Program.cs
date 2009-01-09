using System;
using System.Collections.Generic;
using System.Text;
using ActiveDs;
using System.DirectoryServices;
using Microsoft.CommandLine;
using System.Threading;
using System.Reflection;

namespace RotatePassword
{
    class Program
    {
        /// <summary>
        /// Command-line arguments.
        /// </summary>
        class Args
        {
            [Argument(ArgumentType.Required, HelpText = "Current password.")]
            public string currentpassword = string.Empty;
            [Argument(ArgumentType.AtMostOnce, HelpText = "New password, defaults to the current password.")]
            public string newpassword = string.Empty;
            [Argument(ArgumentType.AtMostOnce, DefaultValue = 1, HelpText = "Number of times to rotate the password, default is 1 (change).")]
            public int times = 1;
            [Argument(ArgumentType.AtMostOnce, DefaultValue = false, HelpText = "Simulate rotation, don't actually do anything.")]
            public bool simulate = false;
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("RotatePassword {0}", Assembly.GetExecutingAssembly().GetName().Version);

                Args parsedArgs = new Args();
                if (Parser.ParseArgumentsWithUsage(args, parsedArgs))
                {
                    if (parsedArgs.times < 1) throw new ArgumentOutOfRangeException("times");

                    if (string.IsNullOrEmpty(parsedArgs.newpassword))
                    {
                        // default to the current password
                        parsedArgs.newpassword = parsedArgs.currentpassword;
                    }

                    // connect to Active Directory
                    IADsADSystemInfo sysInfo = null;
                    DirectoryEntry currentUser = null;

                    if (!parsedArgs.simulate)
                    {
                        sysInfo = new ADSystemInfoClass();
                        Console.WriteLine("Current user: {0}", sysInfo.UserName);
                        currentUser = new DirectoryEntry(string.Format("LDAP://{0}", sysInfo.UserName));
                    }

                    // rotate the password
                    string currentPassword = parsedArgs.currentpassword;
                    for (int i = 1; i <= parsedArgs.times; i++)
                    {
                        // the last iteration sets the final password
                        string newPassword = (i == parsedArgs.times) 
                            ? parsedArgs.newpassword 
                            : RandomPassword.Generate();

                        object[] passwordChangeRequest = new object[] 
                        {
                            currentPassword,
                            newPassword
                        };

                        Console.Write("{0} => {1}", currentPassword, newPassword);
                        if (!parsedArgs.simulate)
                        {
                            currentUser.Invoke("ChangePassword", passwordChangeRequest);
                            currentUser.CommitChanges();
                        }
                        currentPassword = newPassword;
                        Console.WriteLine(" [OK]"); 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
        }
    }
}
