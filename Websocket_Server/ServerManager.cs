using CAH.Backend.Classes;
using CAH.Backend.Interfaces;
using System.Collections.Generic;
using WebSocketSharp;

namespace Websocket_Server
{
    public sealed class ServerManager
    {
        public int ClientCounter { get; set; }
        public List<IPlayer> players = new List<IPlayer>();
        public Dictionary<WebSocket, GamePlayer> GamePlayersWithTheirWebSocket = new Dictionary<WebSocket, GamePlayer>();
        public bool GameIsFull 
        { 
            get 
            {
                return ClientCounter == 3 ? true : false;
            } 
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
