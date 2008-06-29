using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Vestris.ResourceLib
{
    /// <summary>
    /// A variable information block
    /// </summary>
    public class VarTable : ResourceTable
    {
        Kernel32.STRING_OR_VAR_INFO_HEADER _header;
        private string _key;
        private Dictionary<UInt16, UInt16> _languages;

        public string Key
        {
            get
            {
                return _key;
            }
        }

        public Kernel32.STRING_OR_VAR_INFO_HEADER Header
        {
            get
            {
                return _header;
            }
        }

        public Dictionary<UInt16, UInt16> Languages
        {
            get
            {
                return _languages;
            }
        }

        public VarTable(IntPtr lpRes)
        {
            Load(lpRes);
        }

        public void Load(IntPtr lpRes)
        {
            _header = (Kernel32.STRING_OR_VAR_INFO_HEADER)Marshal.PtrToStructure(
                lpRes, typeof(Kernel32.STRING_OR_VAR_INFO_HEADER));
            IntPtr pKey = new IntPtr(lpRes.ToInt32() + Marshal.SizeOf(_header));
            _key = Marshal.PtrToStringUni(pKey);

            _languages = new Dictionary<UInt16, UInt16>();

            IntPtr pVarData = ResourceUtil.Align(pKey.ToInt32() + (_key.Length + 1) * 2);
            IntPtr pVar = pVarData;
            while (pVar.ToInt32() < (pVarData.ToInt32() + _header.wValueLength))
            {
                Kernel32.VAR_HEADER var = (Kernel32.VAR_HEADER) Marshal.PtrToStructure(
                    pVar, typeof(Kernel32.VAR_HEADER));
                _languages.Add(var.wLanguageIDMS, var.wCodePageIBM);
                pVar = new IntPtr(pVar.ToInt32() + Marshal.SizeOf(var));
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
            // key
            w.Write(Encoding.Unicode.GetBytes(_key));
            // null-terminator
            w.Write((UInt16)0);
            // pad fixed info
            ResourceUtil.PadToDWORD(w);

            Dictionary<UInt16, UInt16>.Enumerator languagesEnum = _languages.GetEnumerator();
            while (languagesEnum.MoveNext())
            {
                // id
                w.Write((UInt16) languagesEnum.Current.Key);
                // code page
                w.Write((UInt16) languagesEnum.Current.Value);
            }
        }
    }
}
