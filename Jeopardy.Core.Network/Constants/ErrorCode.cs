using ProtoBuf;

namespace Jeopardy.Core.Network.Constants
{
    [ProtoContract]
    public enum ErrorCode
    {
        Undefined,
        MatchmakerUnreachable,
        BadRequest,
        LobbyNotFound,
        LobbyIsFull,
        InvalidPassword,
    }
}
