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

        public bool Connected { get; private set; }

        private bool _disposed = false;
        private TcpClient _tcpClient;

        public MatchmakerClient() => Reconnect();

        public async Task SendRequestAsync(NetworkRequest request)
        {
            if (TryEstablishConnection())
            {
                try
                {
                    _ = await _tcpClient.SendDataAsync(request);
                }
                catch (IOException e)
                {
                    ResponseReceived?.Invoke(this, new ErrorResponse(request, ErrorCode.MatchmakerUnreachable, e.Message));
                    Connected = false;
                }
            }
            else
            {
                ResponseReceived?.Invoke(this, new ErrorResponse(request, ErrorCode.MatchmakerUnreachable, "Matchmaker is unreachable"));
            }
        }

        public void Reconnect() => Task.Run(() => WaitForResponses());

        private async Task WaitForResponses()
        {
            try
            {
                if (TryEstablishConnection())
                {
                    while (true)
                    {
                        NetworkResponse? response = await _tcpClient.ReceiveDataAsync<NetworkResponse>();
                        ResponseReceived?.Invoke(this, response);
                    }
                }
                else
                {
                    ResponseReceived?.Invoke(this, new ErrorResponse(null, ErrorCode.MatchmakerUnreachable, "Matchmaker is unreachable"));
                }
            }
            catch
            {
                ResponseReceived?.Invoke(this, new ErrorResponse(null, ErrorCode.MatchmakerUnreachable, "Matchmaker is unreachable"));
                Dispose();
            }

            Connected = false;
        }

        public bool TryEstablishConnection()
        {
            try
            {
                if (!Connected)
                {
                    _tcpClient = new(DefaultEndpoint, DefaultPort);
                    Connected = true;
                }

                return true;
            }
            catch
            {
                return false;
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
