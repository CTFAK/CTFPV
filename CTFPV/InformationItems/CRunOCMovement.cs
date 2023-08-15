using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCMovement : PropertyPanel
    {
        public int MovementOffset;

        // Movement Information
        public int ModuleNameOffset;
        public int MovementID;
        public int DataOffset;
        public int DataLength;

        // Movement
        public short Control;
        public short Type;
        public byte Move;
        public byte Opt;
        public int DirectionAtStart;

        public override void InitData(string parentPointer)
        {
            // Movement Information
            ModuleNameOffset = PV.MemLib.ReadInt(parentPointer + ", 0x" + MovementOffset.ToString("X"));
            MovementID = PV.MemLib.ReadInt(parentPointer + ", 0x" + (MovementOffset + 4).ToString("X"));
            DataOffset = PV.MemLib.ReadInt(parentPointer + ", 0x" + (MovementOffset + 8).ToString("X"));
            DataLength = PV.MemLib.ReadInt(parentPointer + ", 0x" + (MovementOffset + 12).ToString("X"));

            // Movement
            Control = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + DataOffset.ToString("X"));
            Type = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (DataOffset + 2).ToString("X"));
            Move = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x" + (DataOffset + 4).ToString("X"));
            Opt = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x" + (DataOffset + 5).ToString("X"));
            DirectionAtStart = PV.MemLib.ReadInt(parentPointer + ", 0x" + (DataOffset + 8).ToString("X"));
        }

        public override void RefreshData(string parentPointer)
        {

        }

        public override List<TreeViewItem> GetPanel()
        {
            return new List<TreeViewItem>();
        }

        public override void RefreshPanel(ref TreeView panel)
        {

        }
    }
}
