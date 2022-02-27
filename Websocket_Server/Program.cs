using CAH.Backend.Classes;
using CAH.Backend.Factories;
using CAH.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Websocket_Server;
using WebSocketSharp.Server;


WebSocketServer wss = new WebSocketServer("ws://localhost:4200");

wss.AddWebSocketService<Echo>("/Echo");
wss.AddWebSocketService<EchoAll>("/EchoAll");
wss.Start();




Console.WriteLine($"WebSocket Server started on http://localhost:4200/EchoAll");

//Wachten op spelers





//GameFactory gameFactory = new GameFactory();

//List<IGamePlayer> CreateGamePlayers(List<IPlayer> players)
//{
//    List<IGamePlayer> _gamePlayers = new List<IGamePlayer>();
//    foreach (var player in players)
//    {
//        GamePlayer gp = new GamePlayer(player);
//        gp.PlayerState = new GamePlayerState();
//        _gamePlayers.Add(gp);
//    }
//    return _gamePlayers;
//}

//Game game = gameFactory.CreateGame(CreateGamePlayers());
//GameManager gameManager = game.GameManager;

//gameManager.StartGame();

Console.ReadKey();

wss.Stop();

