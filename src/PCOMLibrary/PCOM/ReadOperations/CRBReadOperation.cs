using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class CRBReadOperation : ReadOperation<bool>
    {
        public CRBReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.CounterRunBit;
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