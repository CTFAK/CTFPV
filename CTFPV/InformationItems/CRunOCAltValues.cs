using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCAltValues : PropertyPanel
    {
        public int ValuesOffset;
        public int StringsOffset;

        // Alterable Values
        public short AltValueCount;
        public int[] AltValues = new int[0];

        // Alterable Strings
        public short AltStringCount;
        public string[] AltStrings = new string[0];

        public override void InitData(string parentPointer)
        {
            // Alterable Values
            if (ValuesOffset > 0)
            {
                AltValueCount = 0;// (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + ValuesOffset.ToString("X"));
                AltValues = new int[AltValueCount];
                for (int i = 0; i < AltValueCount; i++)
                    AltValues[i] = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ValuesOffset + 2 + (i * 4)).ToString("X"));
            }

            // Alterable Strings
            if (StringsOffset > 0)
            {
                AltStringCount = 0;// (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + StringsOffset.ToString("X"));
                AltStrings = new string[AltStringCount];
                int stringOffset = 2;
                for (int i = 0; i < AltStringCount; i++)
                {
                    AltStrings[i] = PV.MemLib.ReadUnicode(parentPointer + ", 0x" + (StringsOffset + stringOffset).ToString("X"));
                    stringOffset += (AltStrings[i].Length + 1) * 2;
                }
            }
        }

        public override void RefreshData(string parentPointer)
        {
            // Alterable Values
            if (ValuesOffset > 0)
            {
                AltValueCount = 0;// (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + ValuesOffset.ToString("X"));
                AltValues = new int[AltValueCount];
                for (int i = 0; i < AltValueCount; i++)
                    AltValues[i] = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ValuesOffset + 2 + (i * 4)).ToString("X"));
            }

            // Alterable Strings
            if (StringsOffset > 0)
            {
                AltStringCount = 0;// (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + StringsOffset.ToString("X"));
                AltStrings = new string[AltStringCount];
                int stringOffset = 2;
                for (int i = 0; i < AltStringCount; i++)
                {
                    AltStrings[i] = PV.MemLib.ReadUnicode(parentPointer + ", 0x" + (StringsOffset + stringOffset).ToString("X"));
                    stringOffset += (AltStrings[i].Length + 1) * 2;
                }
            }
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
