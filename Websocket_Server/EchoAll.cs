using CAH.Backend.Classes;
using CAH.Backend.Factories;
using CAH.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Timers;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Websocket_Server
{
    public class EchoAll : WebSocketBehavior
    {
        string playerNames = "";
        protected override void OnMessage(MessageEventArgs e)
        {
            ServerManager.Instance.ClientCounter += 1;
            CreatePlayer(e.Data);
            foreach (var player in ServerManager.Instance.players)
            {
                playerNames += player.Name + " ";
            }
            Sessions.Broadcast($"Aantal spelers: {ServerManager.Instance.ClientCounter} {playerNames}");

            if (ServerManager.Instance.GameIsFull)
            {
                Sessions.Broadcast($"Het spel gaat beginnen!");
                CreateGame();
            }
        }

        void CreateGame()
        {
            GameFactory gameFactory = new GameFactory();
            Game game = gameFactory.CreateGame(CreateGamePlayers(ServerManager.Instance.players));
            GameManager gameManager = game.GameManager;

            gameManager.StartGame();
        }

        void CreatePlayer(string playerName)
        {
            Player p = new Player();
            p.Name = playerName; //Input client name
            p.ID = Guid.NewGuid();
            ServerManager.Instance.players.Add(p);
        }

        List<IGamePlayer> CreateGamePlayers(List<IPlayer> players)
        {
            List<IGamePlayer> _gamePlayers = new List<IGamePlayer>();
            foreach (var player in ServerManager.Instance.players)
            {
                GamePlayer gp = new GamePlayer(player);
                gp.PlayerState = new GamePlayerState();
                _gamePlayers.Add(gp);
            }
            return _gamePlayers;
        }
    }
}
