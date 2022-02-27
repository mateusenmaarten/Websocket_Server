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
            ServerManager.Instance.ClientCounter += 1;
            CreatePlayer(e.Data);
            foreach (var player in ServerManager.Instance.players)
            {
                playerNames += player.Name + " ";
            }
            Sessions.Broadcast($"Aantal spelers: {ServerManager.Instance.ClientCounter} {playerNames}"); //Lijst wordt niet bijgehouden
        }

        void CreatePlayer(string playerName)
        {
                Player p = new Player();
                p.Name = playerName; //Input client name
                p.ID = Guid.NewGuid();
                ServerManager.Instance.players.Add(p);
        }
    }
}
