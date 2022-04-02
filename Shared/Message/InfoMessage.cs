namespace Websocket_Server
{
    public class InfoMessage : Message
    {
        public InfoMessage() : base()
        {
            
        }

        public override string MessageType => nameof(InfoMessage);

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
