using CAH.Backend.Interfaces;


namespace Websocket_Server
{
    public class DealWhiteCardMessage : Message
    {
        public DealWhiteCardMessage(IPlayer player, WhiteCard[] playerCards) : base()
        {
            PlayerCards = playerCards;
        }

        public WhiteCard[] PlayerCards { get; }

        public override string MessageType => nameof(DealWhiteCardMessage);

        public override MessageRouting Routing => MessageRouting.ServerToPlayer;
    }

}
