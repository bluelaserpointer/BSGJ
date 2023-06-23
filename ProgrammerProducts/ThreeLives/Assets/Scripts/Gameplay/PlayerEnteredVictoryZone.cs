using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine.SceneManagement;

namespace Platformer.Gameplay
{

    /// <summary>
    /// This event is triggered when the player character enters a trigger with a VictoryZone component.
    /// </summary>
    /// <typeparam name="PlayerEnteredVictoryZone"></typeparam>
    public class PlayerEnteredVictoryZone : Simulation.Event<PlayerEnteredVictoryZone>
    {
        public VictoryZone victoryZone;

        public override void Execute()
        {
            PlatformerModel model = Simulation.GetModel<PlatformerModel>();
            //model.player.animator.SetTrigger("victory"); //TODO: new scene load animation
            model.Player.controlEnabled = false;
            WorldManager.Instance.LoadNewScene(victoryZone.nextSceneName);
        }
    }
}