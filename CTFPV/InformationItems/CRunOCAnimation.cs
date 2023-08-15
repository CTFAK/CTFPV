using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCAnimation : PropertyPanel
    {
        public int AnimationOffset;

        public short[] Offsets = new short[0];

        public override void InitData(string parentPointer)
        {
            Offsets = new short[32];
            for (int i = 0; i < 32; i++)
                Offsets[i] = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (AnimationOffset + (i * 2)).ToString("X"));
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
