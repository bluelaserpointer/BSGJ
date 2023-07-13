using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        public override void Execute()
        {
            PlatformerModel model = Simulation.GetModel<PlatformerModel>();
            var player = model.Player;
            player._animator.SetBool("dead", false);
            player.Collider2d.enabled = true;
            player.controlEnabled = false;
            if (player.AudioSource && player.respawnAudio)
                player.AudioSource.PlayOneShot(player.respawnAudio, 0.5f);
            player.Health.Increment();
            player.Teleport(model.spawnPoint.transform.position);
            player.jumpState = PlayerController.JumpState.Grounded;
            model.virtualCamera.m_Follow = player.transform;
            model.virtualCamera.m_LookAt = player.transform;
            Simulation.Schedule<EnablePlayerInput>(0.7f);
        }
    }
}