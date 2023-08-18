using CTFPV.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTFPV.InformationItems.Events
{
    public class CEvent
    {
        public int EventOffset;

        public short Size;
        public short Type;
        public short Num;
        public short Object;
        public short ObjectList;
        public BitDict Flags = new BitDict(new string[]
        {
            "Repeat",              // 1
            "Done",                // 2
            "Default",             // 4
            "Done Before Fade-In", // 8
            "Not Done In Start",   // 16
            "Always",              // 32
            "Bad",                 // 64
            "Bad Object",          // 128
            "Not",                 // 256
            "Notable",             // 512
            "Monitorable",         // 1024
            "To Delete",           // 2048
            "New Sound"            // 4096
        });
        public byte ParameterCount;
        public byte DefaultType;
        public short Identifier;

        public CEventParameter[] Parameters;

        public void Read(string parentPointer)
        {
            Size = PV.MemLib.ReadShort(parentPointer + ", 0x" + EventOffset.ToString("X"));
            Type = PV.MemLib.ReadShort(parentPointer + ", 0x" + (EventOffset + 2).ToString("X"));
            Num = PV.MemLib.ReadShort(parentPointer + ", 0x" + (EventOffset + 4).ToString("X"));
            Object = PV.MemLib.ReadShort(parentPointer + ", 0x" + (EventOffset + 6).ToString("X"));
            ObjectList = PV.MemLib.ReadShort(parentPointer + ", 0x" + (EventOffset + 8).ToString("X"));
            Flags.flag = PV.MemLib.ReadUShort(parentPointer + ", 0x" + (EventOffset + 10).ToString("X"));
            ParameterCount = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x" + (EventOffset + 12).ToString("X"));
            DefaultType = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x" + (EventOffset + 13).ToString("X"));
            Identifier = PV.MemLib.ReadShort(parentPointer + ", 0x" + (EventOffset + 14).ToString("X"));

            int offset = EventOffset + 16;
            Parameters = new CEventParameter[ParameterCount];
            for (int i = 0; i < ParameterCount; i++)
            {
                CEventParameter parameter = new CEventParameter();
                parameter.ParameterOffset = offset;
                parameter.Read(parentPointer);
                Parameters[i] = parameter;

                offset += parameter.Size;
            }
        }
    }
}
