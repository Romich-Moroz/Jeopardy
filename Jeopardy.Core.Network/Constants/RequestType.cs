using ProtoBuf;

namespace Jeopardy.Core.Network.Constants
{
    [ProtoContract]
    public enum RequestType
    {
        Undefined,
        CreateLobby,
        DeleteLobby,
        GetLobbyList,
        JoinLobby,
        Disconnect,
        NotRequested,
    }
}
