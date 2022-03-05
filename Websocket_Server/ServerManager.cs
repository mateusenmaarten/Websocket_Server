using CAH.Backend.Classes;
using CAH.Backend.Interfaces;
using System.Collections.Generic;
using WebSocketSharp;

namespace Websocket_Server
{
    public sealed class ServerManager
    {
        public int ClientCounter { get; set; }
        public Dictionary<WebSocket, IPlayer> PlayersWaitingForGame = new Dictionary<WebSocket, IPlayer>();
        public Dictionary<WebSocket, GamePlayer> GamePlayersWithTheirWebSocket = new Dictionary<WebSocket, GamePlayer>();

        public int NumberOfPlayersForGame
        {
            get { return 3;}
            set { NumberOfPlayersForGame = value;}
        }
        public bool GameIsFull 
        { 
            get { return ClientCounter == NumberOfPlayersForGame ? true : false;} 
        }

        private ServerManager() { }
        private static ServerManager instance = null;
        public static ServerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServerManager();
                }
                return instance;
            }
        }

    }
}
