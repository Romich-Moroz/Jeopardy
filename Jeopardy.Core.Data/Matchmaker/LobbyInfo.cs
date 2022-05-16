using Jeopardy.Core.Cryptography;
using Jeopardy.Core.Data.Gameplay;
using ProtoBuf;

namespace Jeopardy.Core.Data.Matchmaker
{
    [ProtoContract]
    public class LobbyInfo
    {
        public static int MaxAllowedPlayerCount => 8;
        public static int MinAllowedPlayerCount => 1;

        [ProtoMember(1)]
        public GameState GameState { get; set; } = new();

        [ProtoMember(3)]
        public string NetworkLobbyId { get; set; } = Guid.Empty.ToString();
        [ProtoMember(4)]
        public DateTime LobbyCreationDate { get; set; } = DateTime.Now;
        [ProtoMember(5)]
        public string LobbyName { get; set; } = "Lobby name";
        //[ProtoMember(6)]
        public SecurePassword? Password { get; set; }
        [ProtoMember(7)]
        public int MaxPlayerCount { get; set; } = MaxAllowedPlayerCount;

        public int CurrentPlayerCount => GameState.Players.Count;
        public string OrganizerName => GameState.Host.NetworkIdentity.Username;
        public bool IsPasswordProtected => Password != null;

        public LobbyPreview ToLobbyPreview() => new()
        {
            NetworkLobbyId = NetworkLobbyId,
            LobbyName = LobbyName,
            OrganizerName = OrganizerName,
            MaxPlayerCount = MaxPlayerCount,
            CurrentPlayerCount = CurrentPlayerCount,
            IsPasswordProtected = IsPasswordProtected,
        };
    }
}
