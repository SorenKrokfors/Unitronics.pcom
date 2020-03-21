using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class CCReadOperation : ReadOperation<short>
    {
        public CCReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.CounterCurrent;
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