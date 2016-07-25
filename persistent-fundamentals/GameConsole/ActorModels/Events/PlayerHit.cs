namespace GameConsole.ActorModels.Events
{
    public class PlayerHit
    {
        public int DamageTaken { get; }

        public PlayerHit(int damageTaken)
        {
            DamageTaken = damageTaken;
        }
    }
}