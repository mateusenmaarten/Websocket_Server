using System;
using System.Collections.Generic;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Websocket_Server
{
    public partial class GameManagerServer : WebSocketBehavior
    {
        private WebSocketServer websocketserver;
        private GameServer? gameServer = null;

        public GameManagerServer()
        {
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var incomingMessage = JsonSerializer.Deserialize<UserUpdateNameMessage>(e.Data);
            List<GameOption> validOptions = new List<GameOption>() { GameOption.NewGame, GameOption.JoinGame };

            GameOption chosenOptionEnum = Enum.Parse<GameOption>(incomingMessage.Name);
            
            bool chosenOptionIsValid = validOptions.Contains(chosenOptionEnum);

            if (chosenOptionIsValid)
            {
                switch (chosenOptionEnum)
                {
                    case GameOption.NewGame:
                        string gameServerUrl = CreateNewGameOnServer();
                        Send(gameServerUrl);
                        break;
                    case GameOption.JoinGame:
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

        public string CreateNewGameOnServer()
        {
            //CREATE NEW GAME WITH DUMMY PLAYERS TO USE THE GAMEID AS URL ID, replace players at arrival
            Guid gameGuid = new Guid();
            string gameUrl = Constants.CreateGameUrl(gameGuid);

            

            var gamewss = new WebSocketServer();

            gamewss.AddWebSocketService<GameServer>(gameUrl);

            if (gameServer == null) throw new Exception();

            return gameServer.URL;
        }
        private void SendListOfActiveGamesOnAllServers()
        {
            //foreach (var gameServer in Lobby.Instance.GameServerWithPlayers.Keys)
            //{
            //    Send(gameServer.URL);
            //}
        }

        public void SendPlayerToRequestedGame(Guid id)
        {
            var url = Constants.CreateGameUrl(id);
            Send(url);
        }
    }
}
