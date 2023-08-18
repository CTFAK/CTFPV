using CTFPV.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTFPV.InformationItems.Events
{
    public class CEventParameter
    {
        public int ParameterOffset;

        public short Size;
        public short Code;

        public short W0;
        public short W1;
        public short W2;
        public short W3;
        public short W4;
        public short W5;
        public short W6;
        public short W7;

        public int L0 => W0 << 8 + W1;
        public int L1 => W2 << 8 + W3;
        public int L2 => W4 << 8 + W5;
        public int L3 => W6 << 8 + W7;

        public void Read(string parentPointer)
        {
            Size = PV.MemLib.ReadShort(parentPointer + ", 0x" + ParameterOffset.ToString("X"));
            Code = PV.MemLib.ReadShort(parentPointer + ", 0x" + (ParameterOffset + 2).ToString("X"));
            W0 = PV.MemLib.ReadShort(parentPointer + ", 0x" + (ParameterOffset + 4).ToString("X"));
            W1 = PV.MemLib.ReadShort(parentPointer + ", 0x" + (ParameterOffset + 6).ToString("X"));
            W2 = PV.MemLib.ReadShort(parentPointer + ", 0x" + (ParameterOffset + 8).ToString("X"));
            W3 = PV.MemLib.ReadShort(parentPointer + ", 0x" + (ParameterOffset + 10).ToString("X"));
            W4 = PV.MemLib.ReadShort(parentPointer + ", 0x" + (ParameterOffset + 12).ToString("X"));
            W5 = PV.MemLib.ReadShort(parentPointer + ", 0x" + (ParameterOffset + 14).ToString("X"));
            W6 = PV.MemLib.ReadShort(parentPointer + ", 0x" + (ParameterOffset + 16).ToString("X"));
            W7 = PV.MemLib.ReadShort(parentPointer + ", 0x" + (ParameterOffset + 18).ToString("X"));
        }
    }
}
