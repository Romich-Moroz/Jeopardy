using Jeopardy.Core.Data.Matchmaker;
using System.Collections.Generic;

namespace Jeopardy.Desktop.Client.App.Models.Storages
{
    internal class LobbyInfoStorage
    {
        public LobbyInfo CurrentLobbyInfo { get; set; } = new();
        public List<LobbyPreview> BrowserLobbyList { get; set; } = new();
    }
}
