using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Contexts
{
    [ProtoContract]
    public class WinnerContext : GameContext
    {
        [ProtoMember(1)]
        public List<string> WinnerNetworkUserIds { get; set; } = new List<string>();

        private WinnerContext() { }

        public WinnerContext(List<string> winnerNetworkUserIds) => WinnerNetworkUserIds = winnerNetworkUserIds;
    }
}
