using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Outlook;

namespace OutlookDeleteEmptyFolders
{
    class Program
    {
        static MAPIFolder Find(string path, Folders folders)
        {
            foreach (MAPIFolder childFolder in folders)
            {
                if (childFolder.FullFolderPath == path)
                    return childFolder;

                MAPIFolder found = Find(path, childFolder.Folders);
                if (found != null) return found;
            }

            return null;
        }

        static void DeleteEmptyFolders(MAPIFolder parentFolder)
        {
            foreach (MAPIFolder childFolder in parentFolder.Folders)
            {
                DeleteEmptyFolders(childFolder);
            }

            if (parentFolder.Items.Count == 0 && parentFolder.Folders.Count == 0)
            {
                Console.WriteLine(parentFolder.FullFolderPath);
                parentFolder.Delete();
            }
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    throw new System.Exception("usage: OutlookDeleteEmptyFolders [root]");
                }

                Application application = new Application();
                MAPIFolder root = Find(args[0], application.Session.Folders);
                if (root == null) throw new System.Exception(string.Format("Cannot find '{0}'.", args[0]));
                DeleteEmptyFolders(root);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("ERROR: {0}", ex.Message);
            }
        }
    }
}
