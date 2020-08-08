using System.Collections;
using UnityEngine;

namespace Player_Scripts.Skills
{
    public class Heal : Skill
    {
        public GameObject healthParticles;
        private float healthParticleYOffset = 4f;
        public GameObject greenGlowParticles;
        private float greenGlowParticleYOffset = 0.5f;
        
        private int healAmount = 40;
        private float healDuration = 8f;
        private float tickRate = 0.2f;
        private bool isHealing;

        public override void Cast(Player player)
        {
            if (!CanCast()) return;

            StartCoroutine(HealAnimDelay(player));
            StartCoroutine(HealOverTime(player));

        }

        private IEnumerator HealAnimDelay(Player player)
        {
            player.combatState = Player.CombatState.Combat;
            player.playerInput.DisableInput();
            player.anim.SetTrigger(name);

            var healthParticleGameObject = Instantiate(healthParticles, player.transform.position + new Vector3(0, healthParticleYOffset),
                Quaternion.identity);
            var greenGlowParticleGameObject = Instantiate(greenGlowParticles, player.transform.position + new Vector3(0, greenGlowParticleYOffset),
                Quaternion.identity);
            healthParticleGameObject.transform.parent = player.transform;
            greenGlowParticleGameObject.transform.parent = player.transform;

            yield return new WaitForSeconds(AnimationTimes.instance.HealAnim);
            player.combatState = Player.CombatState.NonCombat;
            player.playerInput.EnableInput();
            yield return null;
        }

        private IEnumerator HealOverTime(Player player)
        {
            // spawn particles
            var healedAmount = 0;
            var healAmountPerTick = Mathf.RoundToInt(healAmount * tickRate / healDuration);
            while (healedAmount < healAmount)
            {
                player.IncreaseHealth(healAmountPerTick);
                healedAmount += healAmountPerTick;
                yield return new WaitForSeconds(tickRate);
            }

            yield return null;
        }
    }
}