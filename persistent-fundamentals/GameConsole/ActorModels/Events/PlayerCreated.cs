namespace GameConsole.ActorModels.Events
{
    public class PlayerCreated
    {
        public PlayerCreated(string playerName, int defaultStartingHealth)
        {
            PlayerName = playerName;
            DefaultStartingHealth = defaultStartingHealth;
        }

        public string PlayerName { get; }
        public int DefaultStartingHealth { get; }
    }
}