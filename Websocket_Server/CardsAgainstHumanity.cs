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
    public class CardsAgainstHumanity : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {

            WebSocket websocket = Context.WebSocket;

            if (e.Data != null && !ServerManager.Instance.GameIsFull)
            {
                ServerManager.Instance.ClientCounter += 1;

                Player playerWaitingForGame = CreatePlayer(e.Data, websocket);
                Send(Environment.NewLine + "CARDS AGAINST HUMANITY");
                Send($"Welkom {playerWaitingForGame.Name} (ID: {playerWaitingForGame.ID})");
            }

            Sessions.Broadcast($"Wachten op spelers ({ServerManager.Instance.ClientCounter}/{ServerManager.Instance.NumberOfPlayersForGame})");
            
            if (ServerManager.Instance.GameIsFull)
            {
                GameManager gameManager = CreateGame();

                Sessions.Broadcast(Environment.NewLine + "Het spel gaat beginnen!");
                Sessions.Broadcast(gameManager.GetCurrentGameState().ToString());

                foreach (var player in ServerManager.Instance.GamePlayersWithTheirWebSocket)
                {
                    string cardsInHand = "";
                    foreach (var card in player.Value.PlayerState.Hand.CardsInHand)
                    {
                        cardsInHand += $"{card.Text}\n";
                    }
                    player.Key.Send($"{player.Value.Player.Name}, dit zijn uw kaarten:\n{cardsInHand}"); 
                }
                // Create new instance of ServerManager singleton? 
            }
        }

        GameManager CreateGame()
        {
            GameFactory gameFactory = new GameFactory();
            List<IGamePlayer> gamePlayers = CreateGamePlayers(ServerManager.Instance.PlayersWaitingForGame);

            Game game = gameFactory.CreateGame(gamePlayers);
            GameManager gameManager = game.GameManager;

            gameManager.StartGame();
            return gameManager;
        }

        Player CreatePlayer(string playerName, WebSocket websocket)
        {
            Player playerWaitingForGame = new Player();

            playerWaitingForGame.Name = playerName;
            playerWaitingForGame.ID = Guid.Parse(ID);

            ServerManager.Instance.PlayersWaitingForGame.Add(websocket,playerWaitingForGame);

            return playerWaitingForGame;
        }

        List<IGamePlayer> CreateGamePlayers(Dictionary<WebSocket,IPlayer> players)
        {
            List<IGamePlayer> _gamePlayers = new List<IGamePlayer>();
            foreach (var player in players)
            {
                GamePlayer gp = new GamePlayer(player.Value);
                gp.PlayerState = new GamePlayerState();
                _gamePlayers.Add(gp);
                ServerManager.Instance.GamePlayersWithTheirWebSocket.Add(player.Key, gp);
            }
            return _gamePlayers;
        }
    }
}
