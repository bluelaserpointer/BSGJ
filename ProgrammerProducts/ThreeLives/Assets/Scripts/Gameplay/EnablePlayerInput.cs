using Platformer.Core;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// This event is fired when user input should be enabled.
    /// </summary>
    public class EnablePlayerInput : Simulation.Event<EnablePlayerInput>
    {
        public override void Execute()
        {
            var player = Simulation.GetModel<PlatformerModel>().Player;
            player.controlEnabled = true;
        }
    }
}