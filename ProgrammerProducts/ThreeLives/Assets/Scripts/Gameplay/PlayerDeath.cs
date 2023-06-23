using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel Model => Simulation.GetModel<PlatformerModel>();
        public override void Execute()
        {
            var player = Model.Player;
            if (player.Health.IsAlive)
            {
                player.Health.Die();
                Model.virtualCamera.m_Follow = null;
                Model.virtualCamera.m_LookAt = null;
                // player.collider.enabled = false;
                player.controlEnabled = false;

                if (player.AudioSource && player.ouchAudio)
                    player.AudioSource.PlayOneShot(player.ouchAudio);
                player._animator.SetTrigger("hurt");
                player._animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
            }
        }
    }
}