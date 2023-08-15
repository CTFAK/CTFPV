using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunBackdrop : PropertyPanel
    {
        // Handles
        public short FrameObjectHandle;
        public short ObjectInfoHandle;

        // Information
        public int X;
        public int Y;
        public short ImageHandle;
        public short ColorMode;
        public short Layer;
        public short ObstacleType;
        public int InkEffect;
        public int InkEffectParam;

        public override void InitData(string parentPointer)
        {
            // Handles
            FrameObjectHandle = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x0");
            ObjectInfoHandle = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x2");

            // Information
            X = PV.MemLib.ReadInt(parentPointer + ", 0x4");
            Y = PV.MemLib.ReadInt(parentPointer + ", 0x8");
            ImageHandle = (short)PV.MemLib.Read2Byte(parentPointer + ", 0xC");
            ColorMode = (short)PV.MemLib.Read2Byte(parentPointer + ", 0xE");
            Layer = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x10");
            ObstacleType = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x12");
            InkEffect = PV.MemLib.ReadInt(parentPointer + ", 0x18");
            InkEffectParam = PV.MemLib.ReadInt(parentPointer + ", 0x1C");
        }

        public override void RefreshData(string parentPointer)
        {
            // Information
            X = PV.MemLib.ReadInt(parentPointer + ", 0x4");
            Y = PV.MemLib.ReadInt(parentPointer + ", 0x8");
            ImageHandle = (short)PV.MemLib.Read2Byte(parentPointer + ", 0xC");
            Layer = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x10");
            InkEffect = PV.MemLib.ReadInt(parentPointer + ", 0x18");
            InkEffectParam = PV.MemLib.ReadInt(parentPointer + ", 0x1C");
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
