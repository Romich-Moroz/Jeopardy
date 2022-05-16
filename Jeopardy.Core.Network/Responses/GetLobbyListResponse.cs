using Jeopardy.Core.Data.Matchmaker;
using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Responses
{
    [ProtoContract]
    public class GetLobbyListResponse : NetworkResponse
    {
        [ProtoMember(1)]
        public List<LobbyPreview> LobbyPreviews { get; set; } = new();

        private GetLobbyListResponse() => RequestType = RequestType.GetLobbyList;

        public GetLobbyListResponse(string requestId, List<LobbyPreview> lobbyPreviews) : this()
        {
            NetworkRequestId = requestId;
            LobbyPreviews = lobbyPreviews;
        }
    }
}
