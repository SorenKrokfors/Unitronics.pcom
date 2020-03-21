using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class MLReadOperation : ReadOperation<int>
    {
        public MLReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.ML;
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