using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class TCReadOperation : ReadOperation<int>
    {
        public TCReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.TimerCurrent;
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