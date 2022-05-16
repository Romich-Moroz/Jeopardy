using Jeopardy.Core.Network.Constants;
using ProtoBuf;

namespace Jeopardy.Core.Network.Requests
{
    [ProtoContract]
    public class GetLobbyListRequest : NetworkRequest
    {
        private GetLobbyListRequest() { }

        public GetLobbyListRequest(string userId)
        {
            RequestType = RequestType.GetLobbyList;
            NetworkUserId = userId;
        }
    }
}
