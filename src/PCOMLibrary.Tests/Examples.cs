using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unitronics.PCOM;
using Unitronics.PCOM.ReadOperations;
using Unitronics.PCOM.WriteOperations;
using Unitronics.Utils;

namespace PCOMLibrary.Tests
{
    [TestClass]
    public class Examples
    {
        [TestMethod]
        public async Task CreateAndSendReciveMessage()
        {
            var  cMessage=new CommunicationMessage();
            var MI10 = new MIReadOperation(10, 1);
            cMessage.AddOperation(MI10);
            cMessage.AddOperation(new MIWriteOperation(12,1));
            var client=new PcomTcpClient(new CancellationTokenSource(),new FileLogger(new FileLoggerSettings
            {
                BytesPerLine = 16,
                FileName = string.Empty, //Use this file to hexdump messages sent
                ShowAscii = false,
                ShowHeader = true,
                ShowOffset = true
            }));
            
            await client.TcpClient.ConnectAsync("192.168.100.101", 20258);
            if (client.TcpClient.Connected)
            {
                var response = await client.SendAndReceive(cMessage.GetMessage());
                cMessage.ParseMessage(response);
                Assert.AreNotEqual(0, MI10.GetValue(0));
            }
            else
            {
                throw  new Exception("Not Connected");
            }
        }
    }
}
