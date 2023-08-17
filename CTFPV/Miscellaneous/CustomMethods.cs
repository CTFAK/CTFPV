using Memory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTFPV.Miscellaneous
{
    public static class CustomMethods
    {
        public static Color ReadColor(this Mem m, string code, bool bgr = false)
        {
            int color = m.ReadInt(code);
            byte[] vals = BitConverter.GetBytes(color);
            if (bgr)
                return Color.FromArgb(vals[3], vals[2], vals[1], vals[0]);
            else
                return Color.FromArgb(vals[3], vals[0], vals[1], vals[2]);
        }

        public static ushort ReadUShort(this Mem m, string code) => (ushort)m.Read2Byte(code);
        public static short ReadShort(this Mem m, string code) => (short)m.Read2Byte(code);
    }
}
