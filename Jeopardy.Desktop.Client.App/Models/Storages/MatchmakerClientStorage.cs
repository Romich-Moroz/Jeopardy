using Jeopardy.Core.Network;

namespace Jeopardy.Desktop.Client.App.Models.Storages
{
    internal class MatchmakerClientStorage
    {
        public MatchmakerClient MatchmakerClient { get; set; } = new();
    }
}
