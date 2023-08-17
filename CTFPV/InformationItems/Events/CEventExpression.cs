using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTFPV.InformationItems.Events
{
    public class CEventExpression
    {
        public short Type;
        public short Num;
        public ushort Size;

        public short Param1;
        public short Param2;
        public short Param3;

        public short eObject => Param1;
        public short eObjectList => Param2;
        public short eNum => Param3;

        public int eIntParam => Param1 << 8 + Param2;

        public double eDoubleParam => BitConverter.Int64BitsToDouble(Param1 << 8 + Param2);
        public float eFloatParam => BitConverter.Int32BitsToSingle(Param3);

        public short eShortParam1 => Param1;
        public short eShortParam2 => Param2;

        public int eExtCode => Param1 << 8 + Param2;
        public short eExtNumber => Param3;
    }
}
