using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class OutputReadOperation : ReadOperation<bool>
    {
        public OutputReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.Output;
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