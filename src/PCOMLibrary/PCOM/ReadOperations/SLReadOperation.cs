using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class SLReadOperation : ReadOperation<int>
    {
        public SLReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.SL;
            NumberOfBytes = 4;
            StartAddress = startAddress;
            NumberOfOperands = numberOfOperands;
        }

        public int GetValue(ushort index)
        {
            if (index > NumberOfOperands) throw new ArgumentOutOfRangeException(nameof(index));
            return Values[index];
        }
    }
}