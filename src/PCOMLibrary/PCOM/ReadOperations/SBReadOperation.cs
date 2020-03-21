using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class SBReadOperation : ReadOperation<bool>
    {
        public SBReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.SB;
            NumberOfBytes = 1;
            StartAddress = startAddress;
            NumberOfOperands = numberOfOperands;
        }

        public bool GetValue(ushort index)
        {
            if (index > NumberOfOperands) throw new ArgumentOutOfRangeException(nameof(index));
            return Values[index];
        }

    }
}