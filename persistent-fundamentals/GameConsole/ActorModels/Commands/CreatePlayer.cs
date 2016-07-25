namespace GameConsole.ActorModels.Commands
{
    public class CreatePlayer
    {
        public CreatePlayer(string playerName, int defaultStartingHealth)
        {
            PlayerName = playerName;
            DefaultStartingHealth = defaultStartingHealth;
        }

        public string PlayerName { get; }
        public int DefaultStartingHealth { get; }
    }
}