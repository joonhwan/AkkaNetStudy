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
        private static readonly ILogger _logger = LogManager.GetLogger("Player");

        private PlayerActorState _state;
        private int _eventCount;

        public PlayerActor(string playerName, int defaultStartingHealth)
        {
            _logger.Trace($"Player {playerName}[health={defaultStartingHealth}] is being created.");

            _state = new PlayerActorState()
                     {
                         PlayerName = playerName,
                         Health = defaultStartingHealth
                     };
            _eventCount = 0;

            Command<HitPlayer>(message =>
            {
                var playerHit = new PlayerHit(message.Damage);
                Persist(playerHit, @event =>
                {
                    _logger.Info($"Player[{_state}] persisted HitMessage(damange={@event.DamageTaken})");
                    HitPlayer(message.Damage);

                    if ((++_eventCount) == 5)
                    {
                        _logger.Trace($"Player[{_state}] saving snap...");
                        SaveSnapshot(_state);
                        _eventCount = 0;
                    }
                });
            });
            Command<DisplayStatus>(message => DisplayStatus());
            Command<SimulateError>(message => CauseError());

            Recover<PlayerHit>(@event =>
            {
                _logger.Info($"Player[{_state}] replayed HitMessage(damange={@event.DamageTaken})");
                HitPlayer(@event.DamageTaken);
            });
            Recover<SnapshotOffer>(offer =>
            {
                _logger.Trace($"Player[{_state}] recovering snapshot..");

                _state = (PlayerActorState)offer.Snapshot;
            });
        }

        public override string PersistenceId => $"player-{_state.PlayerName}";

        private void HitPlayer(int damage)
        {
            _logger.Info($"Player[{_state}] got damage({damage})");
            _state.Health -= damage;

        }

        private void DisplayStatus()
        {
            _logger.Info($"Player State : {_state}");
        }

        private void CauseError()
        {
            _logger.Error($"Player[{_state}] got simulated error.");
            throw new ApplicationException($"Simulated Exception for Player[{_state}]");
        }

    }
}