using System;

namespace Unitronics.PCOM.ReadOperations
{
    public class XBReadOperation : ReadOperation<bool>
    {
        public XBReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.XB;
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