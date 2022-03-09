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
        private string DisplayWelcomeMessage(Player p)
        {
            string welcomeString = Environment.NewLine
                + "CARDS AGAINST HUMANITY\n"
                + "-----------------------\n"
                + $"Welkom {p.Name} (ID: {p.ID})\n";

            return welcomeString;
        }
        private string ShowPlayersInGame(Dictionary<WebSocket, GamePlayer> gamePlayers)
        {
            string playersInGame = $"Spelers:\n";
            foreach (var gp in gamePlayers)
            {
                playersInGame += $" -> { gp.Value.Player.Name}\n";
            }

            return playersInGame;
        }
        private void DisplayOpeningHands(GameManager gm, Dictionary<WebSocket, GamePlayer> gamePlayers)
        {
            foreach (var gp in gamePlayers)
            {
                string openingHand = $"Het spel gaat beginnen!\n";
                openingHand += $"{gp.Value.Player.Name}, dit zijn uw kaarten:\n\n";
                string cardsInHand = "";
                foreach (var card in gp.Value.PlayerState.Hand.CardsInHand)
                {
                    cardsInHand += $" ->{card.Text}\n";
                }
                openingHand += cardsInHand;
                gp.Key.Send(openingHand);
            }
        }
        
        protected override void OnMessage(MessageEventArgs e)
        {

            // incoming message = new game
            // 1. new server manager
            // 2. send url to requesting player


            // incoming message = join game with id
            // 1. resolve existing server manager
            // 2. send url to requesting player

            // server manager has own websocket with it' own url for communicating with players


            WebSocket websocket = Context.WebSocket;

            if (e.Data != null && !ServerManager.Instance.GameIsFull)
            {
                ServerManager.Instance.ClientCounter += 1;

                Player playerWaitingForGame = CreatePlayer(e.Data, websocket);
                
                Send(DisplayWelcomeMessage(playerWaitingForGame));
            }

            Sessions.Broadcast($"Wachten op spelers ({ServerManager.Instance.ClientCounter}/{ServerManager.Instance.NumberOfPlayersForGame})");
            
            if (ServerManager.Instance.GameIsFull)
            {
                GameManager gameManager = CreateGame();

                Sessions.Broadcast(ShowPlayersInGame(ServerManager.Instance.GamePlayersWithTheirWebSocket));

                DisplayOpeningHands(gameManager, ServerManager.Instance.GamePlayersWithTheirWebSocket);

                gameManager.StartNewTurn();

            }
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
        GameManager CreateGame()
        {
            GameFactory gameFactory = new GameFactory();
            List<IGamePlayer> gamePlayers = CreateGamePlayers(ServerManager.Instance.PlayersWaitingForGame);

            Game game = gameFactory.CreateGame(gamePlayers);
            GameManager gameManager = game.GameManager;

            gameManager.StartGame();
            return gameManager;
        }
    }
}
