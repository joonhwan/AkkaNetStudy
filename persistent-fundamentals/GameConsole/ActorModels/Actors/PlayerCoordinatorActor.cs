using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence;
using GameConsole.ActorModels.Messages;
using NLog;

namespace GameConsole.ActorModels.Actors
{
    public class PlayerCoordinatorActor : ReceivePersistentActor
    {
        private static ILogger _logger = LogManager.GetLogger("PlayerCoordinator");

        public PlayerCoordinatorActor()
        {
            Command<CreatePlayerMessage>(_ => Persist(_, message =>
            {
                _logger.Info($"Received {message}");
                CreatePlayer(message.PlayerName, message.DefaultStartingHealth);
            }));
            Recover<CreatePlayerMessage>(message =>
            {
                CreatePlayer(message.PlayerName, message.DefaultStartingHealth);
            });
        }

        private static void CreatePlayer(string playerName, int defaultStartingHealth)
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
