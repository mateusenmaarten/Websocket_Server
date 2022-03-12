using CAH.Backend.Classes;
using CAH.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Websocket_Server
{
    public class Lobby
    {
        public List<string> ActiveGameServers { get; set; }
        public Dictionary<GameServer, IPlayer> GameServerWithPlayers = new Dictionary<GameServer, IPlayer>();

        public int NumberOfPlayersForGame
        {
            get { return 3; }
            set { NumberOfPlayersForGame = value; }
        }
        public bool GameIsFull //Moet per server worden bekeken
        {
            get { return GameServerWithPlayers.Values.Count == NumberOfPlayersForGame ? true : false; }
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
