using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay.Contexts
{
    [ProtoContract]
    public class PlayerSelectContext : GameContext
    {
        [ProtoMember(1)]
        public string SelectorNetworkUserId { get; set; } = string.Empty;

        private PlayerSelectContext() { }

        public PlayerSelectContext(string selectedNetworkUserId) => SelectorNetworkUserId = selectedNetworkUserId;
    }
}
