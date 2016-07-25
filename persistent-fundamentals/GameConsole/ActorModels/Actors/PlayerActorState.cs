namespace GameConsole.ActorModels.Actors
{
    public class PlayerActorState
    {
        public string PlayerName { get; set; }
        public int Health { get; set; }

        public override string ToString()
        {
            return $"[PlayerActorState '{PlayerName}'-'{Health}']";
        }
    }
}