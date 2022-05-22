using Jeopardy.Core.Network.Constants;
using Jeopardy.Core.Network.Extensions;
using Jeopardy.Core.Network.Requests;
using Jeopardy.Core.Network.Responses;
using System.Net.Sockets;

namespace Jeopardy.Core.Network
{
    public class MatchmakerClient : IDisposable
    {
        public event EventHandler<NetworkResponse>? ResponseReceived;

        public const int DefaultPort = 50000;
        public const string DefaultEndpoint = "127.0.0.1";

        private bool _disposed = false;
        private readonly TcpClient _tcpClient = new(DefaultEndpoint, DefaultPort);

        public MatchmakerClient() => Task.Run(() => WaitForResponses());

        public async Task SendRequestAsync(NetworkRequest request)
        {
            try
            {
                await _tcpClient.SendDataAsync(request);
            }
            catch (IOException e)
            {
                ResponseReceived?.Invoke(this, new ErrorResponse(request, ErrorCode.MatchmakerUnreachable, e.Message));
            }
        }

        private async Task WaitForResponses()
        {
            try
            {
                while (true)
                {
                    NetworkResponse? response = await _tcpClient.ReceiveDataAsync<NetworkResponse>();
                    ResponseReceived?.Invoke(this, response);
                }
            }
            catch (IOException e)
            {
                ResponseReceived?.Invoke(this, new ErrorResponse(null, ErrorCode.MatchmakerUnreachable, e.Message));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MatchmakerClient()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _tcpClient.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
