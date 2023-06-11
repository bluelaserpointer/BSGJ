using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a VictoryZone, usually used to end the current game level.
    /// </summary>
    public class LadderZone : MonoBehaviour
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                ++model.player.onLadderCount;
            }
        }
        void OnTriggerExit2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                --model.player.onLadderCount;
            }
        }
    }
}
