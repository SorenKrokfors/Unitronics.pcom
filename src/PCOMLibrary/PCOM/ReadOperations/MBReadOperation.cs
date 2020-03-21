using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class MBReadOperation : ReadOperation<bool>
    {
        public MBReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.MB;
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