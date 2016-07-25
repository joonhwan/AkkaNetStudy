using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence;
using GameConsole.ActorModels.Commands;
using GameConsole.ActorModels.Events;
using NLog;

namespace GameConsole.ActorModels.Actors
{
    public class PlayerActor : ReceivePersistentActor
    {
        private static ILogger _logger = LogManager.GetLogger("Player");
        
        private string _name;
        private int _health;

        public PlayerActor(string playerName, int defaultStartingHealth)
        {
            _logger.Trace($"Player {playerName}[health={defaultStartingHealth}] is being created.");
            
            _name = playerName;
            _health = defaultStartingHealth;

            Command<HitPlayer>(message =>
            {
                var playerHit = new PlayerHit(message.Damage);
                Persist(playerHit, @event =>
                {
                    _logger.Info($"Player[{_name}] persisted HitMessage(damange={@event.DamageTaken})");
                    HitPlayer(message.Damage);
                });
            });
            Command<DisplayStatus>(message => DisplayStatus());
            Command<SimulateError>(message => CauseError());

            Recover<PlayerHit>(@event =>
            {
                _logger.Info($"Player[{_name}] replayed HitMessage(damange={@event.DamageTaken})");
                HitPlayer(@event.DamageTaken);
            });
        }

        public override string PersistenceId => $"player-{_name}";
        
        private void HitPlayer(int damage)
        {
            _logger.Info($"Player[{_name}] got damage({damage})");
            _health -= damage;
            
        }

        private void DisplayStatus()
        {
            _logger.Info($"Player[{_name}] health = {_health}");
        }

        private void CauseError()
        {
            _logger.Error($"Player[{_name}] got simulated error.");
            throw new ApplicationException($"Simulated Exception for Player[{_name}]");
        }

    }
}