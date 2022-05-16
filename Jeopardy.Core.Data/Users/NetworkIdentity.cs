using ProtoBuf;
using System.Net.Sockets;

namespace Jeopardy.Core.Data.Users
{
    [ProtoContract]
    public class NetworkIdentity : UserIdentity
    {
        public TcpClient? TcpClient { get; set; }

        public NetworkIdentity() : base() { }

        public NetworkIdentity(string username, byte[] avatar) : base(username, avatar)
        {
        }

        public NetworkIdentity(UserIdentity userIdentity) : base(userIdentity.Username, userIdentity.Avatar)
        {

        }
    }
}
