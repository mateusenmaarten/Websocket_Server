using CAH.Backend.Classes;
using System;


namespace Websocket_Server
{
    public abstract class Message
    {
        public abstract string MessageType { get; }
        public abstract MessageRouting Routing { get; }
    }

}
