using System;
using System.Threading.Tasks;
using Akka.Actor;
using GameConsole.ActorModels.Messages;
using NLog;

namespace GameConsole.ActorModels.Actors
{
    public class PlayerActor : ReceiveActor
    {
        private static ILogger _logger = LogManager.GetLogger("Player");
        
        private string _name;
        private int _health;

        public PlayerActor(string playerName, int defaultStartingHealth)
        {
            _logger.Trace($"Player {playerName}[health={defaultStartingHealth}] is being created.");
            
            _name = playerName;
            _health = defaultStartingHealth;

            Receive<HitMessage>(message => HitPlayer(message.Damage));
            Receive<DisplayStatusMessage>(message => DisplayStatus());
            Receive<CauseErrorMessage>(message => CauseError());
        }

        private void HitPlayer(int damage)
        {
            _health -= damage;
            _logger.Info($"Player[{_name}] got damage({damage}). Health={_health}");
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