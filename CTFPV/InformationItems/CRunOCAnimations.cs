using Encryption_Key_Finder.InformationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CTFPV.InformationItems
{
    public class CRunOCAnimations : PropertyPanel
    {
        public int AnimationOffset;

        public short Size;
        public short AnimationCount;
        public short[] Offsets = new short[0];
        public CRunOCAnimation[] Animations = new CRunOCAnimation[0];
        public bool[] AnimationExist = new bool[0];

        public override void InitData(string parentPointer)
        {
            Size = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + AnimationOffset.ToString("X"));
            AnimationCount = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (AnimationOffset + 2).ToString("X"));

            Offsets = new short[AnimationCount];
            for (int i = 0; i < AnimationCount; i++)
                Offsets[i] = (short)PV.MemLib.Read2Byte(parentPointer + ", 0x" + (AnimationOffset + 4 + (i * 2)).ToString("X"));

            Animations = new CRunOCAnimation[AnimationCount];
            AnimationExist = new bool[AnimationCount];
            for (int i = 0; i < AnimationCount; i++)
                if (Offsets[i] != 0)
                {
                    Animations[i] = new CRunOCAnimation();
                    Animations[i].AnimationOffset = Offsets[i];
                    Animations[i].InitData(parentPointer);
                    AnimationExist[i] = true;
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
