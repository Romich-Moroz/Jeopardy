using Jeopardy.Core.Network;

namespace Jeopardy.Desktop.Client.App.Models.Storages
{
    internal class MatchmakerClientStorage
    {
        private static MatchmakerClient? _matchmakerClient;
        public MatchmakerClient Client
        {
            get
            {
                if (_matchmakerClient is null)
                {
                    _matchmakerClient = new();
                }
                else if (!_matchmakerClient.Connected)
                {
                    _matchmakerClient.Reconnect();
                }

                return _matchmakerClient;
            }
        }
    }
}
