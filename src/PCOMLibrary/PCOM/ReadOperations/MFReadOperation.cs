using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class MFReadOperation : ReadOperation<int>
    {
        public MFReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.MF;
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