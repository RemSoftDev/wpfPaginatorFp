using System;

namespace Func.Types
{
    public class IntMore0Less65535Exclsv
    {
        private ushort _value = 1;
        public ushort Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (value > 0 && value < ushort.MaxValue)
                {
                    _value = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"You try to set value less than 0 or more than {ushort.MaxValue}");
                }
            }
        }

        public static explicit operator IntMore0Less65535Exclsv(int v)
        {
            return new IntMore0Less65535Exclsv() { Value = Convert.ToUInt16(v) };
        }
    }
}
