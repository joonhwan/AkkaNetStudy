using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using GameConsole.ActorModels.Messages;
using NLog;

namespace GameConsole.ActorModels.Actors
{
    public class PlayerCoordinatorActor : ReceiveActor
    {
        private static ILogger _logger = LogManager.GetLogger("PlayerCoordinator");

        public PlayerCoordinatorActor()
        {
            Receive<CreatePlayerMessage>(message =>
            {
                _logger.Info($"Received {message}");
                Context.ActorOf(
                    Props.Create(() => new PlayerActor(message.PlayerName, message.DefaultStartingHealth)),
                    message.PlayerName
                    );
            });
        }
    }
}
