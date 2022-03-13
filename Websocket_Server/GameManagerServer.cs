using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Websocket_Server
{
    public class GameManagerServer : WebSocketBehavior
    {
        const string baseGameServerUrl = "ws://localhost:4200/CardsAgainstHumanity";

        protected override void OnMessage(MessageEventArgs e)
        {
            List<GameOption> validOptions = new List<GameOption>() { GameOption.NewGame, GameOption.JoinGame };

            GameOption chosenOptionEnum = Enum.Parse<GameOption>(e.Data);
            
            bool chosenOptionIsValid = validOptions.Contains(chosenOptionEnum);

            if (chosenOptionIsValid)
            {
                switch (chosenOptionEnum)
                {
                    case GameOption.NewGame:
                        string gameServerUrl = CreateNewServerForGame();
                        Send(gameServerUrl);
                        break;
                    case GameOption.JoinGame:
                        SendListOfActiveServerUrls();
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

        public enum GameOption : int
        {
            NewGame = 1, 
            JoinGame = 2
        }

        public string CreateNewServerForGame()
        {
            GameServer gameServer = new GameServer();
            return gameServer.URL;
        }
        private void SendListOfActiveServerUrls()
        {
            foreach (var gameServer in Lobby.Instance.GameServerWithPlayers.Keys)
            {
                Send(gameServer.URL);
            }
        }

        public void SendPlayerToRequestedGame(Guid id)
        {
            Send($"ws://localhost:4200/CardsAgainstHumanity/{id}");
        }
    }
}
