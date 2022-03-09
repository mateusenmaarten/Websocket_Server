using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Websocket_Server
{
    public class GameManagerServer : WebSocketBehavior
    {
        const string baseGameServerUrl = "ws://localhost:4200/CardsAgainstHumanity";
        List<string> activeGameServers = new List<string>();
        protected override void OnMessage(MessageEventArgs e)
        {
            List<int> validOptions = new List<int>() { (int)GameOption.NewGame, (int)GameOption.JoinGame };
            
            int.TryParse(e.Data, out int chosenOption);
            GameOption chosenOptionEnum = (GameOption)Enum.ToObject(typeof(GameOption), chosenOption);
            
            bool chosenOptionIsValid = validOptions.Contains(chosenOption) ? true : false;

            if (chosenOptionIsValid)
            {
                switch (chosenOptionEnum)
                {
                    case GameOption.NewGame:
                        string gameServerUrl = CreateNewServerForGame();
                        AddServerToActiveServersList(gameServerUrl);
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
            string gameServerID = Guid.NewGuid().ToString();
            WebSocketServer wss = new WebSocketServer(baseGameServerUrl);
            wss.AddWebSocketService<GameServer>($"/{gameServerID}");
            wss.Start();
             
            return baseGameServerUrl + $"/{gameServerID}";
        }
        private void SendListOfActiveServerUrls()
        {
            foreach (var gameServerUrl in activeGameServers)
            {
                Send(gameServerUrl);
            }
        }

        public void AddServerToActiveServersList(string gameServerUrl)
        {
            if (activeGameServers.Contains(gameServerUrl))
            {
                return;
            }

            activeGameServers.Add(gameServerUrl);
        }
        public void SendPlayerToRequestedGame(Guid id)
        {
            Send($"ws://localhost:4200/CardsAgainstHumanity/{id}");
        }
    }
}
