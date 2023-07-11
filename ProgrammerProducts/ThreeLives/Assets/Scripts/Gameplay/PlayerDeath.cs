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
                    player.AudioSource.PlayOneShot(player.ouchAudio, 0.5f);
                player._animator.SetTrigger("hurt");
                player._animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
                player.velocity.y = 5f;
            }
        }
    }
    /// <typeparam name="PlayerDeathSea"></typeparam>
    public class PlayerDeathSea : Simulation.Event<PlayerDeathSea>
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

                if (player.AudioSource && player.ouchSeaAudio)
                    player.AudioSource.PlayOneShot(player.ouchSeaAudio, 0.5f);
                player._animator.SetTrigger("hurt");
                player._animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
                player.velocity.y = 5f;
                Debug.Log("海");
            }
        }
    }
}