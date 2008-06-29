using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Vestris.ResourceLib
{
    public class StringTable : ResourceTable
    {
        Kernel32.STRING_OR_VAR_INFO_HEADER _header;
        string _key;
        Dictionary<string, StringResource> _strings;

        public string Key
        {
            get
            {
                return _key;
            }
        }

        public Dictionary<string, StringResource> Strings
        {
            get
            {
                return _strings;
            }
        }

        public Kernel32.STRING_OR_VAR_INFO_HEADER Header
        {
            get
            {
                return _header;
            }
        }

        public StringTable(IntPtr lpRes)
        {
            Load(lpRes);
        }

        public void Load(IntPtr lpRes)
        {
            _strings = new Dictionary<string, StringResource>();

            _header = (Kernel32.STRING_OR_VAR_INFO_HEADER) Marshal.PtrToStructure(
                lpRes, typeof(Kernel32.STRING_OR_VAR_INFO_HEADER));

            IntPtr pKey = new IntPtr(lpRes.ToInt32() + Marshal.SizeOf(_header));
            _key = Marshal.PtrToStringUni(pKey);

            IntPtr pChild = ResourceUtil.Align(pKey.ToInt32() + (_key.Length + 1) * 2);
            Kernel32.STRING_OR_VAR_INFO_HEADER pChildInfo = (Kernel32.STRING_OR_VAR_INFO_HEADER) Marshal.PtrToStructure(
                pChild, typeof(Kernel32.STRING_OR_VAR_INFO_HEADER));

            // read strings, each string is in a structure described in http://msdn.microsoft.com/en-us/library/aa909025.aspx
            while (pChild.ToInt32() < (lpRes.ToInt32() + _header.wLength))
            {
                StringResource res = new StringResource(pChild);
                _strings.Add(res.Key, res);
                pChild = ResourceUtil.Align(pChild.ToInt32() + res.Header.wLength);
            }
        }

        public override void Write(BinaryWriter w)
        {
            // long wHeaderLengthPos = w.BaseStream.Position;

            w.Write((UInt16) _header.wLength);
            // wValueLength
            w.Write((UInt16) _header.wValueLength);
            // wType
            w.Write((UInt16) _header.wType);
            // szKey
            w.Write(Encoding.Unicode.GetBytes(_key));
            // pad fixed info
            ResourceUtil.PadToDWORD(w);

            Dictionary<string, StringResource>.Enumerator stringsEnum = _strings.GetEnumerator();
            while (stringsEnum.MoveNext())
            {
                Console.WriteLine("Offset: {0} ({1})", w.BaseStream.Position + 160, stringsEnum.Current.Key);
                stringsEnum.Current.Value.Write(w);
            }

            // write the size of the entire structure
            //long wStringTableEndPos = w.BaseStream.Position;
            //w.Seek((int) wHeaderLengthPos, SeekOrigin.Begin);
            //w.Write((UInt16) (wStringTableEndPos - wHeaderLengthPos));
            //w.Seek(0, SeekOrigin.End);
        }
    }
}
