using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class XDWReadOperation : ReadOperation<int>
    {
        public XDWReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.XDW;
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