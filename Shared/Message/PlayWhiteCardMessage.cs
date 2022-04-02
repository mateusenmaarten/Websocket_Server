using CAH.Backend.Interfaces;


namespace Websocket_Server
{
    public class PlayWhiteCardMessage : Message
    {
        public PlayWhiteCardMessage(IPlayer player, string cardID) : base()
        {
            CardID = cardID;
            Player = player;
        }

        public string CardID { get; set; }
        public IPlayer Player { get; set; }

        public override string MessageType => nameof(PlayWhiteCardMessage);

        public override MessageRouting Routing => MessageRouting.PlayerToServer;

    }

}
