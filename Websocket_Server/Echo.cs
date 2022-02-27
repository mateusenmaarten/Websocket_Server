using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Websocket_Server
{
    public class Echo : WebSocketBehavior
    {
         
        protected override void OnMessage(MessageEventArgs e)
        {
           ServerManager.Instance.ClientCounter += 1;
           Console.WriteLine($"Received massege from client: {e.Data}");
           Send($"I received {e.Data}");
        }
    }
}
