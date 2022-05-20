using ProtoBuf;

namespace Jeopardy.Core.Data.Users
{
    [ProtoContract]
    [ProtoInclude(1, typeof(NetworkIdentity))]
    public class UserIdentity
    {
        [ProtoMember(2)]
        public string NetworkUserId { get; set; } = string.Empty;
        [ProtoMember(3)]
        public string Username { get; set; } = string.Empty;
        [ProtoMember(4)]
        public byte[] Avatar { get; set; } = Array.Empty<byte>();

        protected UserIdentity() { }

        public UserIdentity(string username, byte[]? avatar)
        {
            NetworkUserId = Guid.NewGuid().ToString();
            Username = username;
            Avatar = avatar ?? Array.Empty<byte>();
        }
    }
}
