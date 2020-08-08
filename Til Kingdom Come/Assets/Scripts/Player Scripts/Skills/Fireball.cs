using System.Collections;
using UnityEngine;

namespace Player_Scripts.Skills
{
    public class Fireball : Skill
    {
       
        private float handAnimDelay = 0.4f;
        
        // Fireball
        public GameObject fireBallParticle;
        private float fireballLandDelay = 0.5f;
        private float fireballXOffset = 0.5f;
        private float fireballYOffset = 2.8f;
        
        // Ground fire
        public GameObject groundFire;
        private float groundFireXOffset = 10f; // distance of fire from player


        public override void Cast(Player player)
        {
            if (!CanCast()) return;
            StartCoroutine(FireballAnimDelay(player));
        }

        private IEnumerator FireballAnimDelay(Player player)
        {
            player.anim.SetTrigger(name);
            player.combatState = Player.CombatState.Combat;
            player.playerInput.DisableInput();
            
            // Animation of player raising hand to mouth
            yield return new WaitForSeconds(handAnimDelay);
            SpawnFireBall(player);
            // Delay for fireball particle to reach the ground
            yield return new WaitForSeconds(fireballLandDelay);
            SpawnGroundFire(player);
            yield return new WaitForSeconds(AnimationTimes.instance.FireBallAnim - fireballLandDelay - handAnimDelay);
            player.combatState = Player.CombatState.NonCombat;
            player.playerInput.EnableInput();
            yield return null;
        }

        private void SpawnFireBall(Player player)
        {
            var fireballOffset = player.IsFacingLeft()
                ? new Vector3(fireballXOffset, fireballYOffset, 0)
                : new Vector3(-1 * fireballXOffset, fireballYOffset, 0);
            Instantiate(fireBallParticle, player.transform.position + fireballOffset, player.transform.rotation);
        }

        private void SpawnGroundFire(Player player)
        {
            var groundFireOffset = player.IsFacingLeft()
                ? new Vector3(-groundFireXOffset, 0, 0)
                : new Vector3(groundFireXOffset, 0, 0);
            var groundFireGameObject = Instantiate(groundFire, player.transform.position + groundFireOffset, player.transform.rotation);
        }
    }
}