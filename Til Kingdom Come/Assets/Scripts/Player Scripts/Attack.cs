using System.Collections;
using UnityEngine;

namespace Player_Scripts
{
    public class Attack : Skill, ICharge
    {
        private LayerMask playerLayerMask;
        private Charge charge;
        private int maxCharge = 3;
        private float chargeTime = 4f;
        private int damage = 20;
        private float reactionDelay = 0.3f;
        private float attackDistance = 4.5f;

        private void Start()
        {
            name = "Attack";
            cooldown = chargeTime;
            charge = new Charge(maxCharge, chargeTime);
            playerLayerMask = 1 << 8;
        }

        private void Update()
        {
            Charging();
        }

        public override void Cast(Player player)
        { 
            if (charge.IsChargeEmpty())
            {
                return;
            }

            // Activate dark mask when charge is full
            if (charge.IsFullyCharged())
            {
                nextAvailableTime = Time.time + chargeTime;
            }
            charge.DecreaseCharge();
            StartCoroutine(ComboOneAnimDelay(player));
        }

        #region COMBOS

        private IEnumerator ComboOneAnimDelay(Player player)
        {
            player.combatState = Player.CombatState.Combat;
            player.anim.SetTrigger("Attack");
            yield return new WaitForSeconds(reactionDelay);
            DetectHit(player);
            yield return new WaitForSeconds(AnimationTimes.instance.AttackAnim - reactionDelay);
            player.combatState = Player.CombatState.NonCombat;
        }

        #endregion

        private void DetectHit(Player player)
        {
            var playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            var direction = player.IsFacingLeft()
                ? new Vector2(-1, 0) // left
                : new Vector2(1, 0); // right
            var yOffset = 2f;
            var xOffset = 0f;
            var tempOffset = player.IsFacingLeft()
                ? new Vector2(-xOffset, yOffset)
                : new Vector2(xOffset, yOffset);
            // Debug.DrawRay(playerPosition + tempOffset, direction * attackDistance, Color.red, 3);
            RaycastHit2D[] rayCasts = Physics2D.RaycastAll(playerPosition + tempOffset, direction, attackDistance, playerLayerMask);
            foreach (var ray in rayCasts)
            {
                Player target = ray.collider.GetComponent<Player>();
                if (target == player) continue;
                if (target.combatState == Player.CombatState.Blocking &&
                    Player.IsOpponentFacingPlayer(player, target))
                {
                    // successful block
                }
                else if (target.combatState == Player.CombatState.Rolling)
                {
                    // successful roll, play sound effect
                }
                else if (target.combatState == Player.CombatState.Dead)
                {
                    // ??? play sound effect   
                }
                else
                {
                    // add sfx
                    target.TakeDamage(damage);
                }
                
            }
            
        }
        
        private void Charging()
        {
            if (!charge.IsFullyCharged())
            {
                if (Time.time >= nextAvailableTime)
                {
                    charge.IncreaseCharge();
                    if (!charge.IsFullyCharged())
                    {
                        nextAvailableTime = Time.time + chargeTime;
                    }
                }

            }
        }

        public int GetCurrentCharge()
        {
            return charge.GetCurrentCharge();
        }
    }
}
