using System;

namespace Unitronics.PCOM
{

    public interface IReadOperation
    {}
    public abstract class ReadOperation<T> : Operation<T>,IReadOperation
    {
        public override byte[] GetMessage()
        {
            var data = new byte[4];
            data[0] = (byte)OperationType;
            data[1] = NumberOfOperands;
            var bytes = BitConverter.GetBytes(StartAddress);
            data[2] = bytes[0];
            data[3] = bytes[1];
            return data;
        }
    }
}