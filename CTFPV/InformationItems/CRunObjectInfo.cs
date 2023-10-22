using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunObjectInfo : PropertyPanel
    {
        // Information
        public string Name = string.Empty;
        public ushort Handle;
        public ushort Type;
        public BitDict Flags = new BitDict(new string[]
        {
            "Load On Call",
            "Discardable",
            "Global",
            "To Delete",
            "Current Frame",
            "To Reload",
            "Ignore Load On Call"
        });
        public CRunObjectCommon ObjectCommon;
        public int FileOffset;
        public int LoadFlags;
        public ushort LoadCount;
        public ushort Count;

        // Effects
        public int InkEffect;
        public int InkEffectParam;
        public byte ExtEffect;

        public override void InitData(string parentPointer)
        {
            // Information
            Name = PV.MemLib.ReadUnicode(parentPointer + ", 0x10, 0x0");
            Handle = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x0");
            Type = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x2");
            Flags.flag = (uint)PV.MemLib.Read2Byte(parentPointer + ", 0x4");
            if (Type > 1)
            {
                ObjectCommon = new CRunObjectCommon();
                ObjectCommon.InitData(parentPointer + ", 0x14");
            }
            FileOffset = PV.MemLib.ReadInt(parentPointer + ", 0x18");
            LoadFlags = PV.MemLib.ReadInt(parentPointer + ", 0x1C");
            LoadCount = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x20");
            Count = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x22");

            // Effects
            InkEffect = PV.MemLib.ReadInt(parentPointer + ", 0x8");
            InkEffectParam = PV.MemLib.ReadInt(parentPointer + ", 0xC");
            ExtEffect = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x24, 0x0");
        }

        public override void RefreshData(string parentPointer)
        {
            // Information
            Flags.flag = (uint)PV.MemLib.Read2Byte(parentPointer + ", 0x4");
            if (Type > 1)
                ObjectCommon.RefreshData(parentPointer + ", 0x14");
            Count = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x22");

            // Effects
            InkEffect = PV.MemLib.ReadInt(parentPointer + ", 0x8");
            InkEffectParam = PV.MemLib.ReadInt(parentPointer + ", 0xC");
            ExtEffect = (byte)PV.MemLib.ReadByte(parentPointer + ", 0x24, 0x0");
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
