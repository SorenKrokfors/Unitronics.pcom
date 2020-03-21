using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class TPReadOperation : ReadOperation<int>
    {
        public TPReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.TimerPreset;
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