using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence;
using GameConsole.ActorModels.Commands;
using GameConsole.ActorModels.Events;
using NLog;

namespace GameConsole.ActorModels.Actors
{
    public class PlayerCoordinatorActor : ReceivePersistentActor
    {
        private static ILogger _logger = LogManager.GetLogger("PlayerCoordinator");
        
        public PlayerCoordinatorActor()
        {
            Command<CreatePlayer>(message =>
            {
                _logger.Info($"Received {message}");
                var @event = new PlayerCreated(message.PlayerName, message.DefaultStartingHealth);
                Persist(@event, _ =>
                {
                    _logger.Trace($"PlayerCoordinator persisted a {@event}");

                    CreatePlayer(message.PlayerName, message.DefaultStartingHealth);

                });
            });
            Recover<PlayerCreated>(@event =>
            {
                _logger.Trace($"PlayerCoordinator replayed event : {@event}");
                CreatePlayer(@event.PlayerName, @event.DefaultStartingHealth);
            });
        }
        
        private void CreatePlayer(string playerName, int defaultStartingHealth)
        {
            
                Context.ActorOf(
                    Props.Create(() => new PlayerActor(playerName, defaultStartingHealth)),
                    playerName
                    );
        }

        public override string PersistenceId => "player-coordintaor";

        protected override void OnPersistFailure(Exception cause, object @event, long sequenceNr)
        {
            base.OnPersistFailure(cause, @event, sequenceNr);
        }

        protected override void OnReplaySuccess()
        {
            base.OnReplaySuccess();
        }
        
    }
}
