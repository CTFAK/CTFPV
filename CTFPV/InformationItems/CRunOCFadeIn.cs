using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCFade : PropertyPanel
    {
        public int FadeOffset;

        // Fade
        public int FadeLength;

        public override void InitData(string parentPointer)
        {
            // Fade
            FadeLength = PV.MemLib.ReadInt(parentPointer + ", 0x" + (FadeOffset + 8).ToString("X"));
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
