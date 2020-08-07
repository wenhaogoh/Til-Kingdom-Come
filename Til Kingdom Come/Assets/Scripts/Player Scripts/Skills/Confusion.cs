using System;
using System.Collections;
using UnityEngine;

namespace Player_Scripts.Skills
{
    public class Confusion : Skill
    {
        public GameObject confusionParticles;
        private float confustionParticleYOffset = 3f;
        
        private float confusionDuration = 4f;
        private float pushDistance = 6f;
        private float rayCastYOffset = 2f;
        private LayerMask playerLayerMask;
        
        private void Awake()
        {
            playerLayerMask = 1 << 8;
        }

        public override void Cast(Player player)
        {
            if (!CanCast()) return;

            StartCoroutine(ConfusionAnimDelay(player));
            DetectHit(player);
        }

        private void DetectHit(Player player)
        {
            var direction = player.IsFacingLeft()
                ? new Vector2(-1, 0)
                : new Vector2(1, 0);
            var rayOffset = player.transform.position + new Vector3(0, rayCastYOffset);
            // Debug.DrawRay(rayOffset, direction * 100, Color.cyan, 3);
            RaycastHit2D[] rayCasts =
                Physics2D.RaycastAll(rayOffset, direction, Single.MaxValue, playerLayerMask);
            foreach (var hit2D in rayCasts)
            {
                var target = hit2D.collider.GetComponent<Player>();
                if (target == player) continue;
                StartCoroutine(Confuse(target));
            }
        }

        private IEnumerator Confuse(Player player)
        {
            player.playerInput.InvertMovementKeys();
            
            var confusionParticlesGameObject = Instantiate(confusionParticles, player.transform.position + new Vector3(0, confustionParticleYOffset),
                player.transform.rotation);
            confusionParticlesGameObject.transform.parent = player.transform;
            yield return new WaitForSeconds(confusionDuration);
            player.playerInput.InvertMovementKeys();
            yield return null;
        }
        
        private IEnumerator ConfusionAnimDelay(Player player)
        {
            player.combatState = Player.CombatState.Combat;
            player.anim.SetTrigger(name);
            yield return new WaitForSeconds(AnimationTimes.instance.ConfusionAnim);
            player.combatState = Player.CombatState.NonCombat;
            yield return null;
        }
    }
}