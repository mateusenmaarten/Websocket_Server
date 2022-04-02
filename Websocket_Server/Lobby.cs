using CAH.Backend.Classes;
using CAH.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Websocket_Server
{
    public class Lobby : WebSocketBehavior
    {

        private Dictionary<Game, GameServer> _gamesWithGameServer = new Dictionary<Game, GameServer>();
        private Dictionary<IPlayer, Game> _playersWithGame = new Dictionary<IPlayer, Game>();

        public List<GameServer> gameservers = new List<GameServer>();
        public List<Game> games = new List<Game>();
        public List<IPlayer> players = new List<IPlayer>();

        public bool GameIsFull 
        {
            get { return false; } //TODO - Moet bekeken worden per spel op de gameserver
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var incomingMessage = JsonSerializer.Deserialize<LoginMessage>(e.Data);
            List<MainMenuOption> validOptions = new List<MainMenuOption>() { MainMenuOption.NewGame, MainMenuOption.JoinGame };

            MainMenuOption chosenOptionEnum = Enum.Parse<MainMenuOption>(incomingMessage.Name);

            bool chosenOptionIsValid = validOptions.Contains(chosenOptionEnum);

            if (chosenOptionIsValid)
            {
                switch (chosenOptionEnum)
                {
                    case MainMenuOption.NewGame:
                        string gameServerUrl = CreateNewGameOnServer();
                        Send(gameServerUrl);
                        break;
                    case MainMenuOption.JoinGame:
                        SendListOfActiveGamesOnAllServers();
                        break;
                    default:
                        Send("Please select a valid option.");
                        break;
                }
            }
            else
            {
                Send("Please select a valid option.");
            }
        }
        public void SendPlayerToRequestedGame(Guid id)
        {
            var url = Constants.CreateGameUrl(id);
            Send(url);
        }

        private Lobby() { }
        private static Lobby instance = null;
        public static Lobby Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Lobby();
                }
                return instance;
            }
        }
    }
}
