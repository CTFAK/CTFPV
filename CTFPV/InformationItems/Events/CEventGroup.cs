using CTFPV.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTFPV.InformationItems.Events
{
    public class CEventGroup
    {
        public int EventGroupOffset;

        public short Size;
        public byte ConditionCount;
        public byte ActionCount;
        public BitDict Flags = new BitDict(new string[]
        {
            "Once",          // 1
            "Not Always",    // 2
            "Repeat",        // 4
            "No More",       // 8
            "Shuffle",       // 16
            "Editor Mark",   // 32
            "Undo Mark",     // 64
            "Complex Group", // 128
            "Breakpoint",    // 256
            "Always Clean",  // 512
            "Or In Group",   // 1024
            "Stop In Group", // 2048
            "Or Logical",    // 4096
            "Grouped",       // 8192
            "Inactive",      // 16384
            "No Good"        // 32768
        });
        public short Inhibit;
        public short InhibitCounter;
        public short Identifier;
        public short UndoIdentifier;

        public CEvent[] Conditions;
        public CEvent[] Actions;

        public void Read(string parentPointer)
        {
            Size = (short)(PV.MemLib.ReadShort(parentPointer + ", 0x" + EventGroupOffset.ToString("X")) * -1);
            ConditionCount = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x" + (EventGroupOffset + 2).ToString("X"));
            ActionCount = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x" + (EventGroupOffset + 3).ToString("X"));
            Flags.flag = PV.MemLib.ReadUShort(parentPointer + ", 0x" + (EventGroupOffset + 4).ToString("X"));
            Inhibit = PV.MemLib.ReadShort(parentPointer + ", 0x" + (EventGroupOffset + 6).ToString("X"));
            InhibitCounter = PV.MemLib.ReadShort(parentPointer + ", 0x" + (EventGroupOffset + 8).ToString("X"));
            Identifier = PV.MemLib.ReadShort(parentPointer + ", 0x" + (EventGroupOffset + 10).ToString("X"));
            UndoIdentifier = PV.MemLib.ReadShort(parentPointer + ", 0x" + (EventGroupOffset + 12).ToString("X"));

            int offset = EventGroupOffset + 16;
            Conditions = new CEvent[ConditionCount];
            for (int i = 0; i < ConditionCount; i++)
            {
                CEvent condition = new CEvent();
                condition.EventOffset = offset;
                condition.Read(parentPointer);
                Conditions[i] = condition;

                offset += condition.Size;
            }

            Actions = new CEvent[ActionCount];
            for (int i = 0; i < ActionCount; i++)
            {
                CEvent action = new CEvent();
                action.EventOffset = offset;
                action.Read(parentPointer);
                Actions[i] = action;

                offset += action.Size;
            }
        }
    }
}
