using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class CPReadOperation : ReadOperation<short>
    {
        public CPReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.CounterPreset;
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