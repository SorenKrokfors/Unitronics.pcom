using System;
using System.IO;

namespace Unitronics.PCOM.WriteOperations
{
    public class SDWWriteOperation : WriteOperation<int>
    {
        public SDWWriteOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.SDW;
            NumberOfBytes = 4;
            StartAddress = startAddress;
            NumberOfOperands = numberOfOperands;
        }

        public override byte[] GetMessage()
        {
            if (Values.Count != NumberOfOperands) throw new ArgumentOutOfRangeException($"Numbers of operands differ from length of value list");
            using (var data = new MemoryStream())
            {
                FillHeader(data);
                for (var index = 0; index < NumberOfOperands; index++)
                {
                    var usValue = Values[index];
                    var bytes = BitConverter.GetBytes(usValue);
                    data.Write(bytes, 0, bytes.Length);
                }
                return data.ToArray();
            }
        }
    }
}