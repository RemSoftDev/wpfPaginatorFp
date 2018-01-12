using System;

namespace Func.Types
{
    public class IntMore0Less32767Exclsv
    {
        private short _value = 1;
        public short Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (value > 0 && value < ushort.MaxValue / 2)
                {
                    _value = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"You try to set value less than 0 or more than {ushort.MaxValue / 2}");
                }
            }
        }
    }
}
