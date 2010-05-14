using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading;
using CommandLine;

namespace WmiProcess
{
    class CmdArgs
    {
        [Argument(ArgumentType.Required, HelpText = "Remote machine name.")]
        public string machine;
        [Argument(ArgumentType.Required, HelpText = "Remote machine username.")]
        public string username;
        [Argument(ArgumentType.Required, HelpText="Remote machine password.")]
        public string password;
        [Argument(ArgumentType.Required, HelpText="Command to run.")]
        public string command;
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CmdArgs parsedArgs = new CmdArgs();
                if (!CommandLine.Parser.ParseArgumentsWithUsage(args, parsedArgs))
                    return;

                ConnectionOptions connOptions = new ConnectionOptions();
                connOptions.Impersonation = ImpersonationLevel.Impersonate;
                connOptions.Authentication = AuthenticationLevel.Default;
                connOptions.Username = parsedArgs.username;
                connOptions.Password = parsedArgs.password;
                connOptions.EnablePrivileges = true;
                ManagementScope scope = new ManagementScope(@"\\" + parsedArgs.machine + @"\root\cimv2", connOptions);
                scope.Connect();

                ManagementPath mgmtPath = new ManagementPath("Win32_Process");
                ManagementClass mgmtClass = new ManagementClass(scope, mgmtPath, null);
                ManagementBaseObject mgmtBaseObject = mgmtClass.GetMethodParameters("Create");
                mgmtBaseObject["CommandLine"] = parsedArgs.command;
                ManagementBaseObject outputParams = mgmtClass.InvokeMethod("Create", mgmtBaseObject, null);
                uint pid = (uint) outputParams["processId"];
                Console.Write("Pid: {0}", pid);

                SelectQuery sQuery = new SelectQuery("Win32_Process", string.Format("processId = {0}", pid));
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(sQuery);
                ManagementObjectCollection processes = null;
                while ((processes = searcher.Get()).Count > 0)
                {
                    Console.Write(".");
                    Thread.Sleep(1000);
                }

                Console.WriteLine(", done.");                
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
        }
    }
}
