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

        public static string ReadAscii(this Mem m, string code, int length = -1) => m.ReadString(code, length: (length == -1 ? 9999 : length), stringEncoding: Encoding.ASCII);

        public static string ReadUnicode(this Mem m, string code, int length = -1)
        {
            string output = string.Empty;
            if (length != -1)
                output = m.ReadString(code, length: length, stringEncoding: Encoding.Unicode);
            else
            {
                int offset = 0;
                while (true)
                {
                    short chara = (short)m.Read2ByteMove(code, offset);
                    if (chara == 0) break;
                    output += (char)chara;
                    offset += 2;
                }
            }
            return output;
        }
    }
}
