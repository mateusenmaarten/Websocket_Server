using CAH.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Websocket_Server
{
    public static class GameServerManager 
    {
        public static GameServer CreateNewGameServer()
        {
            return new GameServer();    
        }
        public static string CreateNewGameOnServer(GameServer gameserver)
        {
            //CREATE NEW GAME WITH DUMMY PLAYERS TO USE THE GAMEID AS URL ID, replace players at arrival
            Guid gameGuid = new Guid();
            string gameUrl = Constants.CreateGameUrl(gameGuid);

            var gamewss = new WebSocketServer();

            gamewss.AddWebSocketService<GameServer>(gameUrl);

            if (gameserver == null) throw new Exception();

            return gameserver.Url;
        }
        public static List<Game> GetActiveGamesOnAllServers()
        {
            //foreach (var gameServer in Lobby.Instance.GameServerWithPlayers.Keys)
            //{
            //    Send(gameServer.URL);
            //}
            return new List<Game>();
        }

        public static List<Game> GetActiveGamesOnServer(GameServer gameserver)
        {
            //foreach (var gameServer in Lobby.Instance.GameServerWithPlayers.Keys)
            //{
            //    Send(gameServer.URL);
            //}
            return new List<Game>();
        }

        public static GameServer AddPlayerToGame(IPlayer player, Game game)
        {
            return new GameServer(); //server where the game is active.
        }
    }
}
