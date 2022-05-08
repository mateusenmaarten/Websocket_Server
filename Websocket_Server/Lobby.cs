using CAH.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Websocket_Shared;
using WebSocketSharp;
using System.Text.Json;

namespace Websocket_Server
{
    public class Lobby   //Ontvangt speler en stuurt deze naar een spel via de GameManagerServer
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

        public void WelcomePlayer(string name, WebSocket ws)
        {
            Player playerWaitingForGame = CreatePlayer(name);
            var menu = JsonSerializer.Serialize(new Message_DisplayMainMenu(name));
            var rawMessage = JsonSerializer.Serialize(new RawMessage("Message_DisplayMainMenu", menu));
            ws.Send(rawMessage);
        }

        private string DisplayMainMenu()
        {
            throw new NotImplementedException();
        }

        //List<MainMenuOption> validOptions = new List<MainMenuOption>() { MainMenuOption.NewGame, MainMenuOption.JoinGame };

        //MainMenuOption chosenOptionEnum = Enum.Parse<MainMenuOption>(incomingMessage.Name);

        //bool chosenOptionIsValid = validOptions.Contains(chosenOptionEnum);

        //if (chosenOptionIsValid)
        //{
        //    switch (chosenOptionEnum)
        //    {
        //        case MainMenuOption.NewGame:
        //            //string gameServerUrl = GameServerManager.CreateNewGameOnServer();
        //            //Send(gameServerUrl);
        //            break;
        //        case MainMenuOption.JoinGame:
        //            //SendListOfActiveGamesOnAllServers();
        //            break;
        //        default:
        //            Send("Please select a valid option.");
        //            break;
        //    }
        //}
        //else
        //{
        //    Send("Please select a valid option.");
        //}

        public void SendPlayerToRequestedGame(Guid id, IPlayer player)
        {
            List<Game> activeGames = GameServerManager.GetActiveGamesOnAllServers();
            var game = activeGames.Where(x => x.ID == id).FirstOrDefault();
            if (game == null || player == null) throw new NullReferenceException("Cannot send player to requested game");
            GameServer gameserver = GameServerManager.AddPlayerToGame(player, game);
            var url = $"{gameserver.Url}/{game.ID}"; 
        }

        Player CreatePlayer(string playerName)
        {
            Player playerWaitingForGame = new Player();

            playerWaitingForGame.Name = playerName;
            playerWaitingForGame.ID = Guid.NewGuid();

            return playerWaitingForGame;
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
