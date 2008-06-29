using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Vestris.ResourceLib
{
    public class VarFileInfo : ResourceTable
    {
        Kernel32.STRING_OR_VAR_INFO_HEADER _header;
        string _key;
        Dictionary<string, VarTable> _variables;

        public string Key
        {
            get
            {
                return _key;
            }
        }

        public Dictionary<string, VarTable> Vars
        {
            get
            {
                return _variables;
            }
        }

        public Kernel32.STRING_OR_VAR_INFO_HEADER Header
        {
            get
            {
                return _header;
            }
        }

        public VarFileInfo(IntPtr lpRes)
        {
            Load(lpRes);
        }

        public void Load(IntPtr lpRes)
        {
            _variables = new Dictionary<string, VarTable>();

            _header = (Kernel32.STRING_OR_VAR_INFO_HEADER)Marshal.PtrToStructure(
                lpRes, typeof(Kernel32.STRING_OR_VAR_INFO_HEADER));

            IntPtr pBlockKey = new IntPtr(lpRes.ToInt32() + Marshal.SizeOf(_header));
            _key = Marshal.PtrToStringUni(pBlockKey);

            IntPtr pChild = ResourceUtil.Align(pBlockKey.ToInt32() + (_key.Length + 1) * 2);
            Kernel32.STRING_OR_VAR_INFO_HEADER pChildInfo = (Kernel32.STRING_OR_VAR_INFO_HEADER)Marshal.PtrToStructure(
                pChild, typeof(Kernel32.STRING_OR_VAR_INFO_HEADER));

            while (pChild.ToInt32() < (lpRes.ToInt32() + _header.wLength))
            {
                VarTable res = new VarTable(pChild);
                _variables.Add(res.Key, res);
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
            w.Write((UInt16) 0);
            // pad fixed info
            ResourceUtil.PadToDWORD(w);

            Dictionary<string, VarTable>.Enumerator variablesEnum = _variables.GetEnumerator();
            while (variablesEnum.MoveNext())
            {
                variablesEnum.Current.Value.Write(w);
            }
        }
    }
}
