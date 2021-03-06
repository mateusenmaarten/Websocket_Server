using System;
using Websocket_Server;
using WebSocketSharp.Server;

WebSocketServer wss = new WebSocketServer(Constants.URL);

wss.AddWebSocketService<GameServer>("/CardsAgainstHumanity");

wss.Start();

Console.WriteLine($"WebSocket Server started on {Constants.GameURL}");
Console.ReadKey();

wss.Stop();

public static class Constants
{
    public static int MANAGERPORT = 4200;
    public static int CLIENTPORT = 4201;
    public static string URL => $"ws://localhost:{MANAGERPORT}";
    public static string GameURL => $"{URL}/CardsAgainstHumanity";
}
