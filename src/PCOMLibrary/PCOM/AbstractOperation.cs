namespace Unitronics.PCOM
{
    public abstract class AbstractOperation
    {
        public OperationType OperationType { get; set; }
        public ushort StartAddress { get; set; }
        public byte NumberOfOperands { get; set; }
        public int NumberOfBytes { get; set; }
        public abstract byte[] GetMessage();
        public abstract void Clear();
        public  abstract string GetTextValue();
    }
}