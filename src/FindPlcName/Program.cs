using System;
using System.Threading;
using System.Threading.Tasks;
using Unitronics.PCOM;

namespace FindPlcName
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("PlcName IP Port");
                Console.WriteLine("Example: PlcName 192.168.100.10 20256");
            }
            else
            {
                try
                {
                    var plc = new PcomTcpClient(new CancellationTokenSource(), null);
                    await plc.TcpClient.ConnectAsync(args[1], int.Parse(args[2]));
                    var message = new CommunicationMessage();
                    var plcName = plc.SendAndReceive(message.GetPlcName());
                    Console.WriteLine("PlcName: " + plcName);
                }
                catch (Exception e)
                {

                    Console.WriteLine("Något gick fel!, Kontrollera IP och Port");
                    Console.WriteLine("Example: PlcName 192.168.100.10 20256");
                }
                     
            }

        }
    }
}
