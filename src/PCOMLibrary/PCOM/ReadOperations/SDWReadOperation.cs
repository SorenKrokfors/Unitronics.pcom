using System;

namespace Unitronics.PCOM
{
    public class SDWReadOperation : ReadOperation<int>
    {
        public SDWReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.SDW;
            StartAddress = startAddress;
            NumberOfOperands = numberOfOperands;
            NumberOfBytes = 4;
        }

        public int GetValue(ushort index)
        {
            if (index > NumberOfOperands) throw new ArgumentOutOfRangeException(nameof(index));
            return Values[index];
        }
    }
}