using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCExtension : PropertyPanel
    {
        public int ExtensionOffset;

        // Extension
        public int Size;
        public int Version;
        public int ID;
        public int Private;
        public byte[] Data = new byte[0];

        public override void InitData(string parentPointer)
        {
            // Extension
            Size = PV.MemLib.ReadInt(parentPointer + ", 0x" + ExtensionOffset.ToString("X"));
            Version = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ExtensionOffset + 8).ToString("X"));
            ID = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ExtensionOffset + 12).ToString("X"));
            Private = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ExtensionOffset + 16).ToString("X"));
            if (Size > 20)
            {
                Data = new byte[Size - 20];
                Data = PV.MemLib.ReadBytes(parentPointer + ", 0x" + (ExtensionOffset + 20).ToString("X"), Size - 20);
            }
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
