using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class SFReadOperation : ReadOperation<float>
    {
        public SFReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.SF;
            NumberOfBytes = 4;
            StartAddress = startAddress;
            NumberOfOperands = numberOfOperands;
        }

        public float GetValue(ushort index)
        {
            if (index > NumberOfOperands) throw new ArgumentOutOfRangeException(nameof(index));
            return Values[index];
        }
    }
}