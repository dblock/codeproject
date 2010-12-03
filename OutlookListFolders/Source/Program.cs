using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Outlook;

namespace OutlookListFolders
{
    class Program
    {
        static void ListFolders(Folders coll)
        {
            ListFolders(0, coll);
        }

        static void ListFolders(int level, Folders coll)
        {
            foreach (MAPIFolder childFolder in coll)
            {
                Console.WriteLine("{0}{1}", new String(' ', level), childFolder.FullFolderPath);
                ListFolders(level + 1, childFolder.Folders);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Application application = new Application();
                ListFolders(application.Session.Folders);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
        }
    }
}
