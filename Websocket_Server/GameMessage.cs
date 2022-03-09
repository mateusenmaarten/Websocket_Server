using CAH.Backend.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websocket_Server
{
    public abstract class GameMessage
    {
        public abstract string MessageType { get; }


        public abstract MessageRouting Routing { get; }

    }


    public abstract class AllPlayersMessage : GameMessage
    {
        public AllPlayersMessage() : base()
        {
        }

    }

    public abstract class SinglePlayerMessage : GameMessage
    {
        public SinglePlayerMessage(CAH.Backend.Classes.IGamePlayer player) : base()
        {
            this.Player = player;
        }
        public CAH.Backend.Classes.IGamePlayer Player { get; }

    }

    public enum MessageRouting
    {
        ServerToPlayer,
        PlayerToServer,
        ServerToAllPlayers
    }

    public class UserUpdateNameMessage : GameMessage
    {
        public UserUpdateNameMessage() : base()
        {
        }

        public string Name { get; }

        public override string MessageType => nameof(UserUpdateNameMessage);

        public override MessageRouting Routing => MessageRouting.PlayerToServer;
    }


    public class PlayWhiteCardMessage : SinglePlayerMessage
    {
        public PlayWhiteCardMessage(IGamePlayer player) : base(player)
        {
        }

        public int CardID { get; }

        public override string MessageType => nameof(PlayWhiteCardMessage);

        public override MessageRouting Routing => MessageRouting.PlayerToServer;
    }

    public class DealWhiteCardMessage : SinglePlayerMessage
    {
        public DealWhiteCardMessage(IGamePlayer player, WhiteCard[] playerCards) : base(player)
        {
            PlayerCards = playerCards;
        }

        public WhiteCard[] PlayerCards { get; }

        public override string MessageType => nameof(DealWhiteCardMessage);

        public override MessageRouting Routing => MessageRouting.PlayerToServer;
    }

    public class ShowWhiteCards : AllPlayersMessage
    {
        public ShowWhiteCards(WhiteCard[] cards) : base()
        {
            Cards = cards;
        }

        public int CardID { get; }

        public override string MessageType => nameof(ShowWhiteCards);

        public override MessageRouting Routing => MessageRouting.ServerToAllPlayers;

        public WhiteCard[] Cards { get; }
    }

}
