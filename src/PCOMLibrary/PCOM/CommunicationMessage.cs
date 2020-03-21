using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Unitronics.Exceptions;
using Unitronics.PCOM.ReadOperations;

[assembly: InternalsVisibleTo("Leanders.Unitronics.Tests")]
namespace Unitronics.PCOM
{
    public class CommunicationMessage
    {
        private const byte AsciiEtx = 0x0D;
        private const int NumberOfBytesEthernetHeader = 6;
        private const int NumberOfBytesHeader = 24;
        private const int NumberOfBytesFooter = 3;
        private const int ErrorCode = NumberOfBytesEthernetHeader + 13;

        internal readonly List<AbstractOperation> ReadOperations = new List<AbstractOperation>();
        internal readonly List<AbstractOperation> WriteOperations = new List<AbstractOperation>();

        public int CountWriteOperations()
        {
            return WriteOperations.Count;
        }

        public int CountReadOperations()
        {
            return ReadOperations.Count;
        }

        public void AddOperation(AbstractOperation operation)
        {
            switch (operation)
            {
                case IReadOperation _:
                    //if (ReadOperations.FirstOrDefault(x=> x.StartAddress==operation.StartAddress)!=null) throw new ArgumentException("Start Address conflict");
                    ReadOperations.Add(operation);
                    break;
                case IWriteOperation _:
                    //if (WriteOperations.FirstOrDefault(x => x.StartAddress == operation.StartAddress) != null) throw new ArgumentException("Start Address conflict");
                    WriteOperations.Add(operation);
                    break;
            }
        }

        public void ClearOperations()
        {
            WriteOperations.Clear();
            ReadOperations.Clear();
        }

        public byte[] GetRtc()
        {
            return new byte[] {0x01, 0x00, 0x65, 0x00, 0x08, 0x00, 0x2F, 0x30, 0x30, 0x52, 0x43, 0x46, 0x35, AsciiEtx};
        }

        public static byte[]  SetRtc(DateTime? date=null)
        {
            if (!date.HasValue) date=DateTime.Now;
            var bytes = Encoding.ASCII.GetBytes(SetRtcCommand(date.Value));
            var data = new List<byte>(10)
            {
                0x01,
                0x00,
                0x65,
                0x00,
                22, //Length
                0x00,
                0x2F
            };
            
            data.AddRange(bytes); 
            var s = GetAsciiChecksum(bytes).ToString("X").PadLeft(2, '0');
            var checkSum = Encoding.ASCII.GetBytes(s);
            data.AddRange(checkSum); 
            data.Add(AsciiEtx);
            return data.ToArray();
        }

        internal static string SetRtcCommand(DateTime dateTime)
        {
            var seconds = dateTime.Second.ToString().PadLeft(2, '0');
            var minutes = dateTime.Minute.ToString().PadLeft(2, '0');
            var hours = dateTime.Hour.ToString().PadLeft(2, '0');
            var day = dateTime.Day.ToString().PadLeft(2, '0');
            var month = dateTime.Month.ToString().PadLeft(2, '0');
            var year = dateTime.Year.ToString().PadLeft(2, '0');
            if (year.Length == 4) year = year.Substring(2);
            var dayOfWeek = ((int) (dateTime.DayOfWeek + 1)).ToString().PadLeft(2, '0');
            var cmd = string.Empty;
            cmd += 0.ToString("X").PadLeft(2, '0');
            cmd += "SC";
            cmd += seconds;
            cmd += minutes;
            cmd += hours;
            cmd += dayOfWeek;
            cmd += day;
            cmd += month;
            cmd += year;
            return cmd;
        }

        public DateTime GetRtc(byte[] message)
        {
            var response = Encoding.ASCII.GetString(message);
            response = response.Substring(4, 16);
            const string rtcPattern = "^RC(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})(\\d{2})$";
            if (!Regex.IsMatch(response, rtcPattern)) return DateTime.Now;
            var splits = Regex.Split(response, rtcPattern, RegexOptions.Compiled);
            var seconds = Convert.ToInt32(splits[1]);
            var minutes = Convert.ToInt32(splits[2]);
            var hours = Convert.ToInt32(splits[3]);
            var day = Convert.ToInt32(splits[5]);
            var month = Convert.ToInt32(splits[6]);
            var year = Convert.ToInt32(splits[7]) + 2000;
            return new DateTime(year, month, day, hours, minutes, seconds);
        }

        public byte[] GetPlcName()
        {
            var header = GetBinaryMessage(0x0C, 0);
            var footer = GetFooter(null);
            var byteList = new List<byte>();
            byteList.AddRange(EthernetHeader(header.Length + footer.Length));
            byteList.AddRange(header);
            byteList.AddRange(footer);
            return byteList.ToArray();
        }

        public string GetPlcName(byte[] message)
        {
            return Encoding.ASCII.GetString(GetDetails(message));
        }

        public byte[] GetMessage()
        {
            var details = GetDetails();
            var header = GetBinaryMessage(80, details.Count);
            var footer = GetFooter(details);

            var byteList = new List<byte>();
            byteList.AddRange(EthernetHeader(header.Length + details.Count + footer.Length));
            byteList.AddRange(header);
            byteList.AddRange(details);
            byteList.AddRange(footer);
            return byteList.ToArray();
        }
        internal static byte[] GetDetails(byte[] message)
        {
            if (message[ErrorCode] != 0) throw new MessageException("Error " + message[ErrorCode]);
            var offset = NumberOfBytesEthernetHeader + NumberOfBytesHeader + NumberOfBytesFooter;
            var details = new byte[message.Length - offset];
            Array.Copy(message, NumberOfBytesEthernetHeader + NumberOfBytesHeader, details, 0, message.Length - offset);
            var c = BitConverter.GetBytes(FooterCheckSum(details));
            if (c[0] != message[message.Length - 3]) throw new InvalidChecksumException("Invalid checksum");
            if (c[1] != message[message.Length - 2]) throw new InvalidChecksumException("Invalid checksum");
            return details;
        }
        internal List<byte> GetDetails()
        {
            var detailData=new List<byte>();
            var numberOfReads = BitConverter.GetBytes((ushort) ReadOperations.Count);
            var numberOfWrites = BitConverter.GetBytes((ushort) WriteOperations.Count);
            detailData.AddRange(numberOfReads);
            detailData.AddRange(numberOfWrites);
            foreach (var readOperation in ReadOperations)
            {
                var message = readOperation.GetMessage();
                detailData.AddRange(message);
                if (message.Length % 2L != 0)  detailData.Add(0); 
            }

            foreach (var writeOperation in WriteOperations)
            {
                var message = writeOperation.GetMessage();
                detailData.AddRange(message);
                if (message.Length % 2L != 0)  detailData.Add(0);  
            }

            return detailData;
        }

        /// <summary>
        /// Function for logging messages in plain text
        /// </summary>
        /// <returns></returns>
        public string GetMessageDetails(int messageSize)
        {
            var str=new StringBuilder();
            str.AppendLine("Message size: " + messageSize);
            str.AppendLine("Reading:");
            foreach (var readOperation in ReadOperations)
            {
                str.Append("Operation type=");
                str.Append(readOperation.OperationType.ToString());
                str.Append(" Start Address=");
                str.Append(readOperation.StartAddress);
                str.Append(" Number of operands=");
                str.Append(readOperation.NumberOfOperands);
                str.Append(" Values =" + readOperation.GetTextValue());
                str.AppendLine();
            }

            str.AppendLine("Writing:");
            foreach (var writeOperation in WriteOperations)
            {
                str.Append("Operation type=");
                str.Append(writeOperation.OperationType.ToString());
                str.Append(" Start Address=");
                str.Append(writeOperation.StartAddress);
                str.Append(" Number of operands=");
                str.Append(writeOperation.NumberOfOperands);
                str.Append(" Values =" + writeOperation.GetTextValue());
                str.AppendLine();
            }

            return str.ToString();
        }

        private AbstractOperation GetOperation(int operand,ushort startAddress,byte numberOfOperands)
        {
            if (operand == 1)  return new MBReadOperation(startAddress,numberOfOperands);
            if (operand == 2)  return new SBReadOperation(startAddress, numberOfOperands);
            if (operand == 3)  return new MIReadOperation(startAddress, numberOfOperands);
            if (operand == 4)  return new SIReadOperation(startAddress, numberOfOperands);
            if (operand == 5)  return new MLReadOperation(startAddress, numberOfOperands);
            if (operand == 6)  return new SLReadOperation(startAddress, numberOfOperands);
            if (operand == 7)  return new MFReadOperation(startAddress, numberOfOperands);
            if (operand == 8)  return new SFReadOperation(startAddress, numberOfOperands);
            if (operand == 9)  return new InputReadOperation(startAddress, numberOfOperands);
            if (operand == 10) return new OutputReadOperation(startAddress, numberOfOperands);
            if (operand == 11) return new TRBReadOperation(startAddress, numberOfOperands);
            if (operand == 12) return new CRBReadOperation(startAddress, numberOfOperands);
            if (operand == 16) return new DWReadOperation(startAddress, numberOfOperands);
            if (operand == 32) return new SDWReadOperation(startAddress, numberOfOperands);
            if (operand == 64) return new XBReadOperation(startAddress,numberOfOperands);
            if (operand == 65) return new XIReadOperation(startAddress, numberOfOperands);
            if (operand == 66) return new XLReadOperation(startAddress, numberOfOperands);
            if (operand == 67) return new XDWReadOperation(startAddress, numberOfOperands);
            if (operand == 128) return new TPReadOperation(startAddress, numberOfOperands);
            if (operand == 129) return new TCReadOperation(startAddress, numberOfOperands);
            if (operand == 144) return new CPReadOperation(startAddress, numberOfOperands);
            if (operand == 145) return new CCReadOperation(startAddress, numberOfOperands);
           return null;
        }

        public void ParseMessage(byte[] message)
        {
            GetDetails(message);
            var numberOfBytes = NumberOfBytesEthernetHeader + NumberOfBytesHeader;
            while (numberOfBytes < message.Length - NumberOfBytesFooter)
            {
                numberOfBytes += 2;
                var numberOfReads = BitConverter.ToInt16(message, numberOfBytes);
                numberOfBytes += 2;
                for (var i = 0; i < numberOfReads; i++)
                {
                    var operand = message[numberOfBytes];
                    numberOfBytes += 1;
                    var numberOfOperands = message[numberOfBytes];
                    numberOfBytes += 1;
                    var startAddress = BitConverter.ToUInt16(message, numberOfBytes);
                    numberOfBytes += 2;
                    var operation = ReadOperations.FirstOrDefault(y => y.StartAddress == startAddress && y.OperationType ==(OperationType)operand);
                    if (operation == null)
                    {
                        operation = GetOperation(operand, startAddress, numberOfOperands);
                        AddOperation(operation);
                    }

                    operation.Clear();
                    for (var tt = 0; tt < numberOfOperands; tt++)
                        switch (operation)
                        {
                            case XIReadOperation xiReadOperation:
                            {
                                var val = BitConverter.ToInt16(message, numberOfBytes);
                                xiReadOperation.AddValue(val);
                                numberOfBytes += xiReadOperation.NumberOfBytes;
                                break;
                            }
                            case MIReadOperation miReadOperation:
                            {
                                var val = BitConverter.ToInt16(message, numberOfBytes);
                                miReadOperation.AddValue(val);
                                numberOfBytes += miReadOperation.NumberOfBytes;
                                break;
                            }
                            case MBReadOperation mbReadOperation:
                            {
                                mbReadOperation.AddValue(BitConverter.ToBoolean(message, numberOfBytes));
                                numberOfBytes += mbReadOperation.NumberOfBytes;
                                break;
                            }
                            case XBReadOperation xbReadOperation:
                            {
                                xbReadOperation.AddValue(BitConverter.ToBoolean(message, numberOfBytes));
                                numberOfBytes += xbReadOperation.NumberOfBytes;
                                break;
                            }
                            case STRReadOperation strReadOperation:
                            {
                                strReadOperation.AddValue(message[numberOfBytes]);
                                strReadOperation.AddValue(message[numberOfBytes + 1]);
                                numberOfBytes += 2;
                                break;
                            }
                            case SBReadOperation sbReadOperation:
                            {
                                sbReadOperation.AddValue(BitConverter.ToBoolean(message, numberOfBytes));
                                numberOfBytes += sbReadOperation.NumberOfBytes;
                                break;
                            }

                            case DWReadOperation dwReadOperation:
                            {
                                dwReadOperation.AddValue(BitConverter.ToInt32(message, numberOfBytes));
                                numberOfBytes += dwReadOperation.NumberOfBytes;
                                break;
                            }

                        }
                    if (numberOfBytes % 2 != 0) numberOfBytes++; //Even check
                }
                return;
            }
        }

        private static IEnumerable<byte> EthernetHeader(int messageLength)
        {
            var header = new byte[] {0x01, 0x00, 0x66, 0x01, 0x00, 0x00};
            var length = BitConverter.GetBytes((ushort) messageLength);
            header[4] = length[0];
            header[5] = length[1];
            return header;
        }

        private static byte[] GetBinaryMessage(byte command, int lengthOfDetails)
        {
            var header = new byte[] {0x2F, 0x5F, 0x4F, 0x50, 0x4C, 0x43, 0x00, 0xFE, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
            header[12] = command;
            var l = BitConverter.GetBytes((ushort) lengthOfDetails);
            header[20] = l[0];
            header[21] = l[1];
            var checkSum = GetCheckSum(header);
            header[22] = checkSum[0];
            header[23] = checkSum[1];
            return header;
        }

        private static byte[] GetFooter(List<byte> details)
        {
            var footer = new byte[] {0x00, 0x00, 0x5C};
            if (details == null) return footer;
            var checkSum = BitConverter.GetBytes(FooterCheckSum(details));
            footer[0] = checkSum[0];
            footer[1] = checkSum[1];
            return footer;
        }

        internal static byte GetAsciiChecksum(IEnumerable<byte> message)
        {
            var sum = message.Aggregate(0, (current, b) => current + b);
            return (byte) (sum % 256);
        }

        internal static byte[] GetCheckSum(IReadOnlyList<byte> header)
        {
            var sum = 0;
            for (var x = 0; x < 23; x++) sum += header[x];
            var checkSum = ~(sum % 0x10000) + 1;
            return BitConverter.GetBytes((ushort) checkSum);
        }

        internal static ushort FooterCheckSum(IEnumerable<byte> details)
        {
            var sum = details.Sum(x => (int) x);
            var checkSum = ~(sum % 0x10000) + 1;
            return (ushort) checkSum;
        }
    }
}