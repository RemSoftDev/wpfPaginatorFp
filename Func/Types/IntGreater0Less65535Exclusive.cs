using System;

namespace Func.Types
{
    public class IntGreater0Less65535Exclusive
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
    }
}
