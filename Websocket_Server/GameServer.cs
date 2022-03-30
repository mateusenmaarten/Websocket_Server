using CAH.Backend.Classes;
using CAH.Backend.Factories;
using CAH.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Websocket_Server
{
    public class GameServer : WebSocketBehavior
    {
        //Gameserver managed meerdere games op 1 websocket.
        private string _baseGameServerUrl = Constants.GameURL;
        private Dictionary<WebSocket,IGamePlayer> _gamePlayers = new Dictionary<WebSocket,IGamePlayer>();

        public GameServer() 
        {

        }

        public Guid GameServerID { get; set; }
        public Guid Id { get; set; }
        public string Url { get; set; } 
        public int MaxNumberOfGamesOnServer { get; set; } = 2;

        private string ShowPlayersInGame(IDictionary<IPlayer, GameServer> players)
        {
            string playersInGame = $"Spelers:\n";
            foreach (var player in players)
            {
                if (player.Value.GameServerID == this.GameServerID)
                {
                    playersInGame += $" -> { player.Key.Name}\n";
                }
            }

            return playersInGame;
        }
        private void DisplayOpeningHands(GameManager gm, Dictionary<WebSocket, IGamePlayer> gamePlayers)
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
            WebSocket websocket = Context.WebSocket;

            if (e.Data != null)
            {
                Player playerWaitingForGame = CreatePlayer(e.Data, websocket);
                Lobby.Instance.GameServerWithPlayers.Add(playerWaitingForGame, this);

                var welcomeMessage = JsonSerializer.Serialize(new WelcomeMessage(playerWaitingForGame));

                Send(welcomeMessage);
            }

            //Sessions.Broadcast($"Wachten op spelers ({...}/{...})");

            if (Lobby.Instance.GameIsFull)
            {
                GameManager gameManager = CreateGame();

                Sessions.Broadcast(ShowPlayersInGame(Lobby.Instance._playersWithGame));

                DisplayOpeningHands(gameManager, _gamePlayers);

                gameManager.StartNewTurn();

            }
        }

        Player CreatePlayer(string playerName, WebSocket websocket)
        {
            Player playerWaitingForGame = new Player();

            playerWaitingForGame.Name = playerName;
            playerWaitingForGame.ID = Guid.Parse(ID);

            return playerWaitingForGame;
        }
        GamePlayer CreateGamePlayer(WebSocket websocket, IPlayer player)
        {
            GamePlayer gp = new GamePlayer(player);
            gp.PlayerState = new GamePlayerState();
            _gamePlayers.Add(websocket,gp);
            
            return gp;
        }
        GameManager CreateGame()
        {
            GameFactory gameFactory = new GameFactory();
       
            Game game = gameFactory.CreateGame(_gamePlayers.Values);
            //Set Game ID to ID from game URL ID
            //Create game with dummy players and replace them when new players arrive?
            GameManager gameManager = game.GameManager;

            gameManager.StartGame();
            return gameManager;
        }
    }
}
