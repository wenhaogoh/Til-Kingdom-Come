using System.Collections;
using Player_Scripts.Interfaces;
using UnityEngine;

namespace Player_Scripts.Skills
{
    public class Block : Skill, ICharge
    {
        private Charge charge;
        private int maxCharge = 2;
        private float chargeTime = 4f;

        private void Awake()
        {
            name = "Block";
            info = "Blocks various attacks";
            charge = new Charge(maxCharge, chargeTime);
            cooldown = chargeTime;
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
            StartCoroutine(BlockAnimDelay(player));
        }
        
        private IEnumerator BlockAnimDelay(Player player)
        {
            player.combatState = Player.CombatState.Blocking;
            player.playerInput.DisableInput();
            player.anim.SetTrigger("Block");
            yield return new WaitForSeconds(AnimationTimes.instance.BlockAnim);
            player.combatState = Player.CombatState.NonCombat;
            player.playerInput.EnableInput();
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

        public void RefillCharges()
        {
            charge.RefillCharges();
        }
    }
}