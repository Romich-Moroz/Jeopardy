using ProtoBuf;

namespace Jeopardy.Core.Data.Matchmaker
{
    [ProtoContract]
    public class LobbyPreview
    {
        [ProtoMember(1)]
        public string NetworkLobbyId { get; set; } = string.Empty;
        [ProtoMember(2)]
        public string? LobbyName { get; set; }
        [ProtoMember(3)]
        public string? OrganizerName { get; set; }
        [ProtoMember(4)]
        public int MaxPlayerCount { get; set; }
        [ProtoMember(5)]
        public int CurrentPlayerCount { get; set; }
        [ProtoMember(6)]
        public bool IsPasswordProtected { get; set; }
    }
}
