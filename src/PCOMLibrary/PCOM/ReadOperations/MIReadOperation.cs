using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class MIReadOperation : ReadOperation<short>
    {
        public MIReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.MI;
            NumberOfBytes = 2;
            StartAddress = startAddress;
            NumberOfOperands = numberOfOperands;
        }

        public short GetValue(ushort index)
        {
            if (index > NumberOfOperands) throw new ArgumentOutOfRangeException(nameof(index));
            return Values[index];
        }
    }
}