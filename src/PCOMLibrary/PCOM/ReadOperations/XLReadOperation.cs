using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class XLReadOperation : ReadOperation<int>
    {
        public XLReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.XL;
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