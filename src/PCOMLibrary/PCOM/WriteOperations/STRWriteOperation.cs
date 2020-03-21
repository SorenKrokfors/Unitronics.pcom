using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Unitronics.PCOM.WriteOperations
{
    public class STRWriteOperation : WriteOperation<string>
    {
        public STRWriteOperation(ushort startAddress, byte numberOfOperands)
        {
            OperationType = OperationType.XI;
            NumberOfBytes = 2;
            StartAddress = startAddress;
            NumberOfOperands = numberOfOperands;
        }

        public override byte[] GetMessage()
        {
            using (var data = new MemoryStream())
            {
                FillHeader(data);
                try
                {
                    var strBytes = Encoding.ASCII.GetBytes(Values.First().Substring(0, Values.First().Length));
                    data.Write(strBytes, 0, strBytes.Length);
                    return data.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public override void AddValue(string value)
        {
            if (value.Length > NumberOfBytes * NumberOfOperands) throw new ArgumentException("String too long");
            Values.Add(value);
        }
    }
}