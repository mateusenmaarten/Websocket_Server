using CAH.Backend.Classes;
using CAH.Backend.Interfaces;
using System;


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
        public SinglePlayerMessage(IPlayer player) : base()
        {
            this.Player = player;
        }
        public IPlayer Player { get; }

    }

    public abstract class ModeSelectionMessage : GameMessage
    {
        public ModeSelectionMessage() : base()
        {
            
        }
        

    }

    public enum MessageRouting
    {
        ServerToPlayer,
        PlayerToServer,
        ServerToAllPlayers
    }

    public class UserUpdateNameMessage : GameMessage
    {
        public UserUpdateNameMessage(string name) : base()
        {
            Name = name;
        }

        public string Name { get; }

        public override string MessageType => nameof(UserUpdateNameMessage);

        public override MessageRouting Routing => MessageRouting.PlayerToServer;

    }

    public class WelcomeMessage : SinglePlayerMessage
    {
        public WelcomeMessage(IPlayer player) : base(player)
        {
            
        }

        public override string MessageType => nameof(WelcomeMessage);

        public override MessageRouting Routing => MessageRouting.ServerToPlayer;

        public string Message 
        {
            get
            {
                string welcomeString = Environment.NewLine
                + "CARDS AGAINST HUMANITY\n"
                + "-----------------------\n"
                + $"Welkom {Player.Name} (ID: {Player.ID})\n";

                return welcomeString;
            }
        }
    }

    public class PlayWhiteCardMessage : SinglePlayerMessage
    {
        public PlayWhiteCardMessage(IPlayer player, string cardID) : base(player)
        {
            CardID = cardID;
        }

        public string CardID { get; }

        public override string MessageType => nameof(PlayWhiteCardMessage);

        public override MessageRouting Routing => MessageRouting.PlayerToServer;

    }

    public class DealWhiteCardMessage : SinglePlayerMessage
    {
        public DealWhiteCardMessage(IPlayer player, WhiteCard[] playerCards) : base(player)
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
