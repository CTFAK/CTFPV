using Encryption_Key_Finder.InformationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCString : PropertyPanel
    {
        public int StringOffset;

        // String
        public int Size;
        public int Width;
        public int Height;
        public int ParagraphCount;

        public int[] ParagraphOffsets = new int[0];
        public CRunOCParagraph[] Paragraphs = new CRunOCParagraph[0];

        public override void InitData(string parentPointer)
        {
            // String
            Size = PV.MemLib.ReadInt(parentPointer + ", 0x" + StringOffset.ToString("X"));
            Width = PV.MemLib.ReadInt(parentPointer + ", 0x" + (StringOffset + 4).ToString("X"));
            Height = PV.MemLib.ReadInt(parentPointer + ", 0x" + (StringOffset + 8).ToString("X"));
            ParagraphCount = PV.MemLib.ReadInt(parentPointer + ", 0x" + (StringOffset + 12).ToString("X"));

            ParagraphOffsets = new int[ParagraphCount];
            Paragraphs = new CRunOCParagraph[ParagraphCount];

            for (int i = 0; i < ParagraphCount; i++)
            {
                ParagraphOffsets[i] = PV.MemLib.ReadInt(parentPointer + ", 0x" + (StringOffset + 16 + (i * 4)).ToString("X"));
                Paragraphs[i] = new CRunOCParagraph();
                Paragraphs[i].ParagraphOffset = ParagraphOffsets[i];
                Paragraphs[i].InitData(parentPointer);
            }
        }

        public override void RefreshData(string parentPointer)
        {
            // String
            Width = PV.MemLib.ReadInt(parentPointer + ", 0x" + (StringOffset + 4).ToString("X"));
            Height = PV.MemLib.ReadInt(parentPointer + ", 0x" + (StringOffset + 8).ToString("X"));
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
