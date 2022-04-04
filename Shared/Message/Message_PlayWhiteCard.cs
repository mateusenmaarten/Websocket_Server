using CAH.Backend.Interfaces;


namespace Websocket_Server
{
    public class Message_PlayWhiteCard : Message
    {
        public Message_PlayWhiteCard(IPlayer player, string cardID) : base()
        {
            CardID = cardID;
            Player = player;
        }

        public string CardID { get; set; }
        public IPlayer Player { get; set; }

        public override string MessageType => nameof(Message_PlayWhiteCard);

        public override MessageRouting Routing => MessageRouting.PlayerToServer;

    }

}
