using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCSubApp : PropertyPanel
    {
        public int SubAppOffset;

        // Sub-Application
        public int Size;
        public int Width;
        public int Height;
        public short Version;
        public short StartFrame;
        public BitDict Options = new BitDict(new string[]
        {
            "Share Global Values",
            "Share Lives",
            "Share Scores",
            "Share Window Attribute",
            "Stretch",
            "Popup",
            "Caption",
            "Tool Caption",
            "Border",
            "Window Resize",
            "System Menu",
            "Disable Close",
            "Modal",
            "Dialogue Frame",
            "Internal",
            "Hide On Close",
            "Custom Size",
            "Internal About Box",
            "Clip Siblings",
            "Share Player Controls",
            "MDI Child",
            "Docked",
            "Docking Area",
            "Docked Left",
            "Docked Top",
            "Docked Right",
            "Docked Bottom",
            "Reopen",
            "MDI Run Even If Not Active",
            "Hidden At Start"
        });
        public string Name;

        public override void InitData(string parentPointer)
        {
            // Sub-Application
            Size = PV.MemLib.ReadInt(parentPointer + ", 0x" + SubAppOffset.ToString("X"));
            Width = PV.MemLib.ReadInt(parentPointer + ", 0x" + (SubAppOffset + 4).ToString("X"));
            Height = PV.MemLib.ReadInt(parentPointer + ", 0x" + (SubAppOffset + 8).ToString("X"));
            Version = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (SubAppOffset + 12).ToString("X"));
            StartFrame = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (SubAppOffset + 14).ToString("X"));
            Options.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (SubAppOffset + 16).ToString("X"));
            Name = PV.MemLib.ReadString(parentPointer + ", 0x" + (SubAppOffset + 20).ToString("X"), length: 255, stringEncoding: Encoding.Unicode);
        }

        public override void RefreshData(string parentPointer)
        {
            // Sub-Application
            Width = PV.MemLib.ReadInt(parentPointer + ", 0x" + (SubAppOffset + 4).ToString("X"));
            Height = PV.MemLib.ReadInt(parentPointer + ", 0x" + (SubAppOffset + 8).ToString("X"));
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
