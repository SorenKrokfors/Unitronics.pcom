using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class SIReadOperation : ReadOperation<short>
    {
        public SIReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.SI;
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