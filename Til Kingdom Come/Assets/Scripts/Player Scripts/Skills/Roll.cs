using System.Collections;
using UnityEngine;

namespace Player_Scripts.Skills
{
    public class Roll : Skill
    {
        private float rollSpeed = 25f;
        
        private void Awake()
        {
            name = "Roll";
            info = "Rolls a short distance, invulnerable while rolling";
            cooldown = 5f;
        }
        
        public override void Cast(Player player)
        {
            if (!CanCast()) return;
            
            AudioController.instance.PlaySoundEffect("Roll");
            StartCoroutine(RollAnimDelay(player));
        }

        private IEnumerator RollAnimDelay(Player player)
        {
            player.combatState = Player.CombatState.Rolling;
            player.anim.SetTrigger(name);
            player.rb.AddForce(player.transform.right * rollSpeed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(AnimationTimes.instance.RollAnim);
            player.combatState = Player.CombatState.NonCombat;
            player.rb.velocity = Vector2.zero;
        }
    }
}