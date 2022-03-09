using System;
using WebSocketSharp.Server;


WebSocketServer wss = new WebSocketServer("ws://localhost:4200");

wss.AddWebSocketService<Websocket_Server.GameManagerServer>("/CardsAgainstHumanity");
wss.Start();

Console.WriteLine($"WebSocket Server started on http://localhost:4200/CardsAgainstHumanity");
Console.ReadKey();

wss.Stop();

