using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Vestris.ResourceLib
{
    public class StringFileInfo : ResourceTable
    {
        Kernel32.STRING_OR_VAR_INFO_HEADER _header;
        string _key;
        Dictionary<string, StringTable> _strings;

        public string Key
        {
            get
            {
                return _key;
            }
        }

        public Dictionary<string, StringTable> Strings
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

        public StringFileInfo(IntPtr lpRes)
        {
            Load(lpRes);
        }

        public void Load(IntPtr lpRes)
        {
            _strings = new Dictionary<string, StringTable>();

            _header = (Kernel32.STRING_OR_VAR_INFO_HEADER) Marshal.PtrToStructure(
                lpRes, typeof(Kernel32.STRING_OR_VAR_INFO_HEADER));

            IntPtr pBlockKey = new IntPtr(lpRes.ToInt32() + Marshal.SizeOf(_header));
            _key = Marshal.PtrToStringUni(pBlockKey);

            IntPtr pChild = ResourceUtil.Align(pBlockKey.ToInt32() + (_key.Length + 1) * 2);
            Kernel32.STRING_OR_VAR_INFO_HEADER pChildInfo = (Kernel32.STRING_OR_VAR_INFO_HEADER)Marshal.PtrToStructure(
                pChild, typeof(Kernel32.STRING_OR_VAR_INFO_HEADER));

            // read strings, each string is in a structure described in http://msdn.microsoft.com/en-us/library/aa909025.aspx
            while (pChild.ToInt32() < (lpRes.ToInt32() + _header.wLength))
            {
                StringTable res = new StringTable(pChild);
                _strings.Add(res.Key, res);
                pChild = ResourceUtil.Align(pChild.ToInt32() + res.Header.wLength);
            }
        }

        public override void Write(BinaryWriter w)
        {
            // wLength
            w.Write((UInt16) _header.wLength);
            // wValueLength
            w.Write((UInt16) _header.wValueLength);
            // wType
            w.Write((UInt16) _header.wType);
            // write key
            w.Write(Encoding.Unicode.GetBytes(_key));
            // null-terminator
            w.Write((UInt16)0);
            // pad fixed info
            ResourceUtil.PadToDWORD(w);

            Dictionary<string, StringTable>.Enumerator stringsEnum = _strings.GetEnumerator();
            while (stringsEnum.MoveNext())
            {
                stringsEnum.Current.Value.Write(w);
            }
        }
    }
}
