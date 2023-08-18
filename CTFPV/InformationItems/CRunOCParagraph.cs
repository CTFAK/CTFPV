using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCParagraph : PropertyPanel
    {
        public int ParagraphOffset;

        // Paragraph
        public short Font;
        public BitDict Flags = new BitDict(new string[]
        {
            "Left Alligned",
            "Horizontal Center Alligned",
            "Right Alligned",
            "Vertical Center Alligned",
            "", "", "", "", "",
            "Correct",
            "Relief"
        });
        public System.Drawing.Color Color = System.Drawing.Color.White;
        public string Text = string.Empty;

        public override void InitData(string parentPointer)
        {
            // Paragraph
            Font = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + ParagraphOffset.ToString("X"));
            Flags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (ParagraphOffset + 2).ToString("X"));
            Color = PV.MemLib.ReadColor(parentPointer + ", 0x" + (ParagraphOffset + 4).ToString("X"));
            Text = PV.MemLib.ReadUnicode(parentPointer + ", 0x" + (ParagraphOffset + 8).ToString("X"));
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
