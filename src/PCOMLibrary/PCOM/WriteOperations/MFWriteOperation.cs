﻿using System;
using System.IO;

namespace Unitronics.PCOM.WriteOperations
{
    public class MFWriteOperation : WriteOperation<int>
    {
        public MFWriteOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.MF;
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
                foreach (var usValue in Values)
                {
                    var bytes = BitConverter.GetBytes(usValue);
                    data.Write(bytes, 0, bytes.Length);
                }
                return data.ToArray();
            }
        }
    }
}