namespace Websocket_Server
{
    public class Message_Login : Message
    {
        public Message_Login(string name) : base()
        {
            Name = name;
        }

        public string Name { get; }

        public override string MessageType => nameof(Message_Login);

        public override MessageRouting Routing => MessageRouting.PlayerToServer;

    }

}
