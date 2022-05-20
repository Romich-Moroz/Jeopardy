//Player.cs

using Jeopardy.Core.Data.Users;
using ProtoBuf;

namespace Jeopardy.Core.Data.Gameplay
{
    [ProtoContract]
    public class Player
    {
        public string Username => NetworkIdentity.Username;
        public string NetworkUserId => NetworkIdentity.NetworkUserId;

        [ProtoMember(1)]
        public NetworkIdentity NetworkIdentity { get; set; } = new();
        [ProtoMember(2)]
        public int Score { get; set; }
        [ProtoMember(3)]
        public bool HasAnswerAttempt { get; set; } = true;
        [ProtoMember(4)]
        public bool IsWinner { get; set; } = false;

        public Player() { }
        public Player(UserIdentity userIdentity) => NetworkIdentity = new(userIdentity);
    }
}
