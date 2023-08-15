using CTFPV.Miscellaneous;
using Encryption_Key_Finder.InformationItems;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCCounters : PropertyPanel
    {
        public int CountersOffset;

        // Counters
        public int Size;
        public int Width;
        public int Height;
        public short Player;
        public short DisplayType;
        public BitDict DisplayFlags = new BitDict(new string[]
        {
            "", "", "", "", "", "", "", "", "",
            "Inverse"
        });
        public short Font;

        // Digits / Anim
        public short FrameCount;
        public short[] Frames = new short[0];

        // Bars / Text
        public short BorderSize;
        public System.Drawing.Color BorderColor = System.Drawing.Color.White;
        public short Shape;
        public short FillType;

        // Line Shape
        public BitDict LineFlags = new BitDict(new string[]
        {
            "1"
        });

        // Solid / Gradient Fill Type
        public System.Drawing.Color Color1 = System.Drawing.Color.White;
        public System.Drawing.Color Color2 = System.Drawing.Color.White;
        public BitDict GradientFlags = new BitDict(new string[]
        {
            "Vertical Gradient"
        });

        public override void InitData(string parentPointer)
        {
            // Counters
            Size = PV.MemLib.ReadInt(parentPointer + ", 0x" + CountersOffset.ToString("X"));
            Width = PV.MemLib.ReadInt(parentPointer + ", 0x" + (CountersOffset + 4).ToString("X"));
            Height = PV.MemLib.ReadInt(parentPointer + ", 0x" + (CountersOffset + 8).ToString("X"));
            Player = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (CountersOffset + 12).ToString("X"));
            DisplayType = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (CountersOffset + 14).ToString("X"));
            DisplayFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (CountersOffset + 16).ToString("X"));
            Font = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (CountersOffset + 18).ToString("X"));

            switch (DisplayType)
            {
                case 1: // Digits
                case 4: // Anim
                    FrameCount = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (CountersOffset + 20).ToString("X"));
                    Frames = new short[FrameCount];
                    for (int i = 0; i < FrameCount; i++)
                        Frames[i] = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (CountersOffset + 22 + (i * 2)).ToString("X"));
                    break;
                case 2: // Vertical Bar
                case 3: // Horizontal Bar
                case 5: // Text
                    BorderSize = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (CountersOffset + 20).ToString("X"));
                    BorderColor = PV.MemLib.ReadColor(parentPointer + ", 0x" + (CountersOffset + 22).ToString("X"));
                    Shape = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (CountersOffset + 24).ToString("X"));
                    FillType = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (CountersOffset + 26).ToString("X"));

                    if (Shape == 1) // Line Shape
                        LineFlags.flag = (ushort)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (CountersOffset + 28).ToString("X"));
                    else
                    {
                        switch (FillType)
                        {
                            case 1: // Solid Fill Type
                                Color1 = PV.MemLib.ReadColor(parentPointer + ", 0x" + (CountersOffset + 28).ToString("X"));
                                break;
                            case 2: // Gradient Fill Type
                                Color1 = PV.MemLib.ReadColor(parentPointer + ", 0x" + (CountersOffset + 28).ToString("X"));
                                Color1 = PV.MemLib.ReadColor(parentPointer + ", 0x" + (CountersOffset + 32).ToString("X"));
                                GradientFlags.flag = (uint)PV.MemLib.ReadInt(parentPointer + ", 0x" + (CountersOffset + 36).ToString("X"));
                                break;
                        }
                    }
                    break;
            }
        }

        public override void RefreshData(string parentPointer)
        {
            // Counters
            Width = PV.MemLib.ReadInt(parentPointer + ", 0x" + (CountersOffset + 4).ToString("X"));
            Height = PV.MemLib.ReadInt(parentPointer + ", 0x" + (CountersOffset + 8).ToString("X"));
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
