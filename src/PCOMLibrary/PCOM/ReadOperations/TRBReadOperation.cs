using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class TRBReadOperation : ReadOperation<bool>
    {
        public TRBReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.TimerRunBit;
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