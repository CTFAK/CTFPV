using Encryption_Key_Finder.InformationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCCounter : PropertyPanel
    {
        public int CounterOffset;

        // Counter
        public int InitialValue;
        public int MinimumValue;
        public int MaximumValue;

        public override void InitData(string parentPointer)
        {
            // Counter
            InitialValue = PV.MemLib.ReadInt(parentPointer + ", 0x" + (CounterOffset + 2).ToString("X"));
            MinimumValue = PV.MemLib.ReadInt(parentPointer + ", 0x" + (CounterOffset + 6).ToString("X"));
            MaximumValue = PV.MemLib.ReadInt(parentPointer + ", 0x" + (CounterOffset + 10).ToString("X"));
        }

        public override void RefreshData(string parentPointer)
        {
            // Counter
            InitialValue = PV.MemLib.ReadInt(parentPointer + ", 0x" + (CounterOffset + 2).ToString("X"));
            MinimumValue = PV.MemLib.ReadInt(parentPointer + ", 0x" + (CounterOffset + 6).ToString("X"));
            MaximumValue = PV.MemLib.ReadInt(parentPointer + ", 0x" + (CounterOffset + 10).ToString("X"));
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
