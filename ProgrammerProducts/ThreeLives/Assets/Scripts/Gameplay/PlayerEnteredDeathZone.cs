using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a player enters a trigger with a DeathZone component.
    /// </summary>
    /// <typeparam name="PlayerEnteredDeathZone"></typeparam>
    public class PlayerEnteredDeathZone : Simulation.Event<PlayerEnteredDeathZone>
    {
        public override void Execute()
        {
            Simulation.Schedule<PlayerDeath>(0);
        }
    }
    /// <typeparam name="PlayerEnteredDeathZoneSea"></typeparam>
    public class PlayerEnteredDeathZoneSea : Simulation.Event<PlayerEnteredDeathZoneSea>
    {
        public override void Execute()
        {
            Simulation.Schedule<PlayerDeathSea>(0);
        }
    }
}