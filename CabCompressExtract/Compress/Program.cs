using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using CabLib;

namespace CompressDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new Exception("syntax: compress [path]");
                }

                string[] files = Directory.GetFiles(args[0], "*.*", SearchOption.AllDirectories);
                Console.WriteLine("Embedding {0} files.", files.Length);
                ArrayList filesArray = new ArrayList(files.Length);
                foreach (string file in files)
                {
                    string relativePath = file.Replace(args[0], "").TrimStart('\\');
                    Console.WriteLine(relativePath);
                    filesArray.Add(new string[] { file,  relativePath});
                }

                string cabname = Path.Combine(Environment.CurrentDirectory, "DEMO_%d.CAB");
                Console.WriteLine("Writing {0} file(s) to {1}", files.Length, cabname);

                Compress cab = new Compress();
                long totalSize = 0;
                long totalFiles = 0;
                cab.evFilePlaced += delegate(string s_File, int s32_FileSize, bool bContinuation)
                {
                    if (!bContinuation)
                    {
                        totalFiles++;
                        totalSize += s32_FileSize;
                        Console.WriteLine(String.Format(" {0} - {1}", s_File, s32_FileSize));
                    }

                    return 0;
                };

                cab.CompressFileList(filesArray, cabname, true, true, 64 * 1024 * 1024);
                Console.WriteLine("Compressed {0} files, {1} byte(s)", totalFiles, totalSize);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ERROR: {0}", ex.Message);
            }
        }
    }
}
