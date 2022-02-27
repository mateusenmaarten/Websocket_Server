using System;
using Websocket_Server;
using WebSocketSharp.Server;


WebSocketServer wss = new WebSocketServer("ws://localhost:4200");

wss.AddWebSocketService<Echo>("/Echo");
wss.AddWebSocketService<EchoAll>("/EchoAll");
wss.Start();




Console.WriteLine($"WebSocket Server started on http://localhost:4200/EchoAll");



Console.ReadKey();

wss.Stop();

