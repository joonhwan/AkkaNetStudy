using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence;
using GameConsole.ActorModels.Messages;
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
            
            Command<HitMessage>(message =>
            {
                Persist(message, savedMessage =>
                {
                    _logger.Info($"Player[{_name}] persisted HitMessage(damange={savedMessage.Damage})");
                    HitPlayer(message.Damage);
                });
            });
            Command<DisplayStatusMessage>(message => DisplayStatus());
            Command<CauseErrorMessage>(message => CauseError());

            Recover<HitMessage>(message =>
            {
                _logger.Info($"Player[{_name}] replayed HitMessage(damange={message.Damage})");
                HitPlayer(message.Damage);
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