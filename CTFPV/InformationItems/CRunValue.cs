using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunValue : PropertyPanel
    {
        public int ValueOffset;

        // Information
        public uint Type;
        public uint Paddle;

        // Value
        public int IntValue;
        public double DoubleValue;
        public string StringValue = string.Empty;

        public object Value()
        {
            // Value
            switch (Type)
            {
                default:
                    return "";
                case 0:
                    return IntValue;
                case 1:
                    return StringValue;
                case 2:
                    return DoubleValue;
            }
        }

        public override void InitData(string parentPointer)
        {
            // Information
            Type = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x" + ValueOffset.ToString("X"));
            Paddle = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x" + (ValueOffset + 4).ToString("X"));

            // Value
            switch (Type)
            {
                case 0:
                    IntValue = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ValueOffset + 8).ToString("X"));
                    break;
                case 1:
                    StringValue = PV.MemLib.ReadUnicode(parentPointer + ", 0x" + (ValueOffset + 8).ToString("X") + ", 0x0");
                    break;
                case 2:
                    DoubleValue = PV.MemLib.ReadDouble(parentPointer + ", 0x" + (ValueOffset + 8).ToString("X"));
                    break;
            }
        }

        public override void RefreshData(string parentPointer)
        {
            // Information
            Type = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x" + ValueOffset.ToString("X"));

            // Value
            switch (Type)
            {
                case 0:
                    IntValue = PV.MemLib.ReadInt(parentPointer + ", 0x" + (ValueOffset + 8).ToString("X"));
                    break;
                case 1:
                    StringValue = PV.MemLib.ReadUnicode(parentPointer + ", 0x" + (ValueOffset + 8).ToString("X") + ", 0x0");
                    break;
                case 2:
                    DoubleValue = PV.MemLib.ReadDouble(parentPointer + ", 0x" + (ValueOffset + 8).ToString("X"));
                    break;
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
