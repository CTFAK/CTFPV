using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCMovements : PropertyPanel
    {
        public int MovementOffset;

        public int MovementCount;
        public CRunOCMovement[] Movements = new CRunOCMovement[0];

        public override void InitData(string parentPointer)
        {
            MovementCount = PV.MemLib.ReadInt(parentPointer + ", 0x" + MovementOffset.ToString("X"));
            Movements = new CRunOCMovement[MovementCount];
            for (int i = 0; i < MovementCount; i++)
            {
                Movements[i] = new CRunOCMovement();
                Movements[i].MovementOffset = MovementOffset + 4 + (i * 16);
                Movements[i].InitData(parentPointer);
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
