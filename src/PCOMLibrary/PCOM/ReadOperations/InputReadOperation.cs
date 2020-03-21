using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class InputReadOperation : ReadOperation<bool>
    {
        public InputReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.Input;
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