using NLog;

namespace GameConsole.ActorModels.Messages
{
    public class CreatePlayerMessage
    {
        public CreatePlayerMessage(string playerName, int defaultStartingHealth)
        {
            PlayerName = playerName;
            DefaultStartingHealth = defaultStartingHealth;
        }

        public string PlayerName { get; }
        public int DefaultStartingHealth { get; }
    }

    public class HitMessage
    {
        public HitMessage(int damage)
        {
            Damage = damage;
        }

        public int Damage { get; }
    }

    public class DisplayStatusMessage
    {
        
    }

    public class CauseErrorMessage
    {
        
    }
}