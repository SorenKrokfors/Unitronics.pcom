namespace Unitronics.PCOM.ReadOperations
{
    public class STRReadOperation : ReadOperation<byte>
    {
        public STRReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.XI;
            NumberOfBytes = 1;
            StartAddress = startAddress;
            NumberOfOperands = numberOfOperands;
        }
    }
}