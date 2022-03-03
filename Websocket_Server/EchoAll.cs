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
            WebSocket websocket = Context.WebSocket;

            if (e.Data != null && !ServerManager.Instance.GameIsFull)
            {
                ServerManager.Instance.ClientCounter += 1;
                Player p = CreatePlayer(e.Data);
                p.ID = Guid.Parse(ID);
                Send($"Welkom {p.Name} (ID: {p.ID})");
            }
            
            foreach (var player in ServerManager.Instance.players)
            {
                playerNames += player.Name + " ";
            }

            Sessions.Broadcast($"Aantal spelers: {ServerManager.Instance.ClientCounter} {playerNames}");
            GamePlayer gameplayer = new GamePlayer(ServerManager.Instance.players[ServerManager.Instance.ClientCounter -1]);
            
            ServerManager.Instance.GamePlayersWithTheirWebSocket.Add(websocket, gameplayer);

            if (ServerManager.Instance.GameIsFull)
            {
                GameManager gameManager = CreateGame();

                Sessions.Broadcast($"Het spel gaat beginnen!");
                Sessions.Broadcast(gameManager.GetCurrentGameState().ToString());

                foreach (var player in ServerManager.Instance.GamePlayersWithTheirWebSocket)
                {
                    
                    gameManager.DrawOpeningHand(player.Value);
                    string cardsInHand = "";
                    foreach (var card in player.Value.PlayerState.Hand.CardsInHand)
                    {
                        cardsInHand += $"{card.Text}\n";
                    }
                    player.Key.Send($"{player.Value.Player.Name}, dit zijn uw kaarten:\n{cardsInHand}"); 
                }
            }
        }

        GameManager CreateGame()
        {
            GameFactory gameFactory = new GameFactory();
            Game game = gameFactory.CreateGame(CreateGamePlayers(ServerManager.Instance.players));
            GameManager gameManager = game.GameManager;

            gameManager.StartGame();
            return gameManager;
        }

        Player CreatePlayer(string playerName)
        {
            Player p = new Player();
            p.Name = playerName; //Input client name
            //p.ID = Guid.NewGuid();
            ServerManager.Instance.players.Add(p);
            return p;
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
