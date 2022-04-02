namespace Websocket_Server
{
    public class LoginMessage : Message
    {
        public LoginMessage(string name) : base()
        {
            Name = name;
        }

        public string Name { get; }

        public override string MessageType => nameof(LoginMessage);

        public override MessageRouting Routing => MessageRouting.PlayerToServer;

    }

}
