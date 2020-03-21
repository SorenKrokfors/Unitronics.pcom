using System;
using System.Buffers;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Unitronics.Utils;

namespace Unitronics.PCOM
{
    public class PcomTcpClient:IDisposable
    {
        public int Port { get; set; } = 20258;
        private readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Create(600, 5);

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly FileLogger _logger;
        public string PlcName { get; set; }

        public TcpClient TcpClient { get; set; }=new TcpClient();

        public PcomTcpClient(CancellationTokenSource cancellationTokenSource,FileLogger logger)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _logger = logger;
        }
        public async Task<byte[]> SendAndReceive(byte[] message)
        {
            byte[] result=null;
            try
            {
                _logger.Log(message, "From PC");
                await TcpClient.GetStream().WriteAsync(message, 0, message.Length, _cancellationTokenSource.Token).ConfigureAwait(false);
                result = _arrayPool.Rent(600);
                var requestedBytes = 6;
                var bytesReceived = 0;
                while (bytesReceived < requestedBytes)
                {
                    bytesReceived += await TcpClient.GetStream().ReadAsync(result, bytesReceived, requestedBytes-bytesReceived, _cancellationTokenSource.Token).ConfigureAwait(false);
                    if (bytesReceived == 0) throw new SocketException();
                    if (bytesReceived == 6) //Got Header
                        requestedBytes = BitConverter.ToInt16(result, 4); //Data length at position 4&5 in eth.header
                }
                var response = new byte[bytesReceived];
                Array.Copy(result, response, bytesReceived);
                _logger.Log(response, "FROM PLC (" + PlcName + ")");
                return response;
            }
            finally
            {
               if (result!=null) _arrayPool.Return(result);
            }
          
        }
        public async Task<byte[]> SendAndReceiveAscii(byte[] message)
        {
            _logger.Log(message, "From PC");
            await TcpClient.GetStream().WriteAsync(message, 0, message.Length, _cancellationTokenSource.Token).ConfigureAwait(false);
            var data = new byte[1];

            using var response = new MemoryStream();
            while (data[0] != 0x0D )
            {
                await TcpClient.GetStream().ReadAsync(data, 0, 1).ConfigureAwait(false);
                response.WriteByte(data[0]);
            }

            var responseData = response.ToArray();
            _logger.Log(responseData, "FROM PLC (" + PlcName + ")");
            return responseData;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TcpClient?.Dispose();
            }
        }

    }
}
