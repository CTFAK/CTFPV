using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunFrameObject : PropertyPanel
    {
        public int ObjectOffset;

        // Handles
        public ushort FrameObjectHandle;
        public ushort Handle;

        // Information
        public int X;
        public int Y;
        public ushort ParentType;
        public ushort ObjectInfoParentType;
        public ushort Layer;
        public ushort Type;

        public override void InitData(string parentPointer)
        {
            // Handles
            FrameObjectHandle = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + ObjectOffset.ToString("X"));
            Handle = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (ObjectOffset + 2).ToString("X"));

            // Information
            X = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ObjectOffset + 4).ToString("X"));
            Y = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ObjectOffset + 8).ToString("X"));
            ParentType = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (ObjectOffset + 12).ToString("X"));
            ObjectInfoParentType = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (ObjectOffset + 14).ToString("X"));
            Layer = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (ObjectOffset + 16).ToString("X"));
            Type = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (ObjectOffset + 18).ToString("X"));
        }

        public override void RefreshData(string parentPointer)
        {
            // Information
            X = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ObjectOffset + 4).ToString("X"));
            Y = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ObjectOffset + 8).ToString("X"));
            Layer = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (ObjectOffset + 16).ToString("X"));
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
