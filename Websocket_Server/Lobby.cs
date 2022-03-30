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

        private Dictionary<Game, GameServer> _gamesWithGameServer = new Dictionary<Game, GameServer>();
        private Dictionary<IPlayer, Game> _playersWithGame = new Dictionary<IPlayer, Game>();

        public List<GameServer> gameservers = new List<GameServer>();
        public List<Game> games = new List<Game>();
        public List<IPlayer> players = new List<IPlayer>();

        public bool GameIsFull 
        {
            get { return false; } //TODO - Moet bekeken worden per spel op de gameserver
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
