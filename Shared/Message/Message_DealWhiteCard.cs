using CAH.Backend.Interfaces;


namespace Websocket_Server
{
    public class Message_DealWhiteCard : Message
    {
        public Message_DealWhiteCard(IPlayer player, WhiteCard[] playerCards) : base()
        {
            PlayerCards = playerCards;
        }

        public WhiteCard[] PlayerCards { get; }

        public override string MessageType => nameof(Message_DealWhiteCard);

        public override MessageRouting Routing => MessageRouting.ServerToPlayer;
    }

}
