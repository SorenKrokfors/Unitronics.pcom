namespace Unitronics.PCOM.ReadOperations
{
    public class DWReadOperation : ReadOperation<int>
    {
        public DWReadOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.DW;
            StartAddress = startAddress;
            NumberOfOperands = numberOfOperands;
            NumberOfBytes = 4;
        }
    }
}