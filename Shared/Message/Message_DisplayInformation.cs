namespace Websocket_Server
{
    public class Message_DisplayInformation : Message
    {
        public Message_DisplayInformation() : base()
        {
            
        }

        public override string MessageType => nameof(Message_DisplayInformation);

        public override MessageRouting Routing => MessageRouting.ServerToPlayer;

        public string Message 
        {
            get
            {
                string welcomeString = Environment.NewLine
                + "CARDS AGAINST HUMANITY\n"
                + "-----------------------\n"
                + $"Welkom!";
                //+$"Welkom {player.Name} (ID: {player.ID})\n";

                return welcomeString;
            }
        }
    }

}
