using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using GameConsole.ActorModels.Actors;
using GameConsole.ActorModels.Messages;
using NLog;
using NLog.Config;

namespace GameConsole
{
    class Program
    {
        private static ILogger _logger = LogManager.GetLogger("Program");

        static void Main(string[] args)
        {
            LogManager.Configuration = new XmlLoggingConfiguration("NLog.config");

            var system = ActorSystem.Create("Game");
            var coordinator = system.ActorOf(Props.Create(() => new PlayerCoordinatorActor()), "PlayerCoordinator");
            while (true)
            {
                var action = ReadLine();
                var path = "";
                try
                {
                    switch (action?[0].ToUpper())
                    {
                    case "C":
                        coordinator.Tell(new CreatePlayerMessage(action[1], 100));
                        break;
                    case "H":
                        path = $"/user/PlayerCoordinator/{action[1]}";
                        system.ActorSelection(path)
                              .Tell(new HitMessage(int.Parse(action[2])));
                        break;
                    case "E":
                        path = $"/user/PlayerCoordinator/{action[1]}";
                        system.ActorSelection(path).Tell(new CauseErrorMessage());
                        break;
                    case "D":
                        path = $"/user/PlayerCoordinator/{action[1]}";
                        system.ActorSelection(path).Tell(new DisplayStatusMessage());
                        break;
                    default:
                        _logger.Warn("Command : C(Create), H(Hit), E(Error)...");
                        break;
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error in Processing Command.");
                    _logger.Warn("Command : C(Create), H(Hit), E(Error)...");
                    //throw;
                }
            }

            //coordinator.Tell(new CreatePlayerMessage();
        }

        private static string[] ReadLine()
        {
            Console.ResetColor();
            var input = Console.ReadLine();
            return input?.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
