using System;
using System.Collections;
using UnityEngine;

namespace Player_Scripts.Skills
{
    public class Heal : Skill
    {
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
            player.anim.SetTrigger(name);
            yield return new WaitForSeconds(AnimationTimes.instance.ConfusionAnim);
            player.combatState = Player.CombatState.NonCombat;
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