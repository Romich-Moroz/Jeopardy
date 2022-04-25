using System.Net;

namespace Jeopardy.Core.Data.Matchmaker
{
    public class MatchmakerLobbyInfo
    {
        private readonly IPAddress HostIpAddress;

        public static readonly TimeSpan LobbyInfoExpirationSpan = TimeSpan.FromMinutes(5);

        public Guid Guid { get; set; } = Guid.NewGuid();
        public DateTime LobbyCreationDate { get; set; } = DateTime.Now;
        public DateTime LobbyExpirationDate { get; set; } = DateTime.Now + LobbyInfoExpirationSpan;
        public string OrganizerName { get; set; }
        public string LobbyName { get; set; }
        public int MaxPlayerCount { get; set; }
        public int CurrentPlayerCount { get; set; } = 0;
        public bool IsPasswordProtected { get; set; }
        public string HostConnectionIpAddress => HostIpAddress.ToString();

        public MatchmakerLobbyInfo(string lobbyName, string organizerName, int maxPlayers, bool isPasswordProtected, IPAddress hostIpAddress)
        {
            LobbyName = lobbyName;
            OrganizerName = organizerName;
            MaxPlayerCount = maxPlayers;
            IsPasswordProtected = isPasswordProtected;
            HostIpAddress = hostIpAddress;
        }
    }
}
