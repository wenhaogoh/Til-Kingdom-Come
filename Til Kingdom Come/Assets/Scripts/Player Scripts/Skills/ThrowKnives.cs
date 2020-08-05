using System;
using System.Collections;
using UnityEngine;

namespace Player_Scripts.Skills
{
    public class ThrowKnives : Skill, ICharge
    {
        private int maxCharge = 2;
        private float chargeTime = 4f;
        private Charge charge;
        public GameObject knife;
        private float speed = 20f;
        private float xOffset = 1.5f;
        private float yOffset = 2f;
        private float knifeDelay = 0.6f;
        
        private void Awake()
        {
            charge = new Charge(maxCharge, chargeTime);
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
            StartCoroutine(ThrowKnivesAnimDelay(player));


        }

        private IEnumerator ThrowKnivesAnimDelay(Player player)
        {
            player.combatState = Player.CombatState.Combat;
            player.anim.SetTrigger(name);
            yield return new WaitForSeconds(knifeDelay);

            var tempOffset = player.IsFacingLeft()
                ? new Vector3(-xOffset, yOffset, 0)
                : new Vector3(xOffset, yOffset, 0);
            var direction = player.IsFacingLeft()
                ? new Vector2(-1, 0)
                : new Vector2(1, 0);
            var rotation = player.IsFacingLeft()
                ? Quaternion.Euler(0, 180, 0)
                : Quaternion.identity;
            var knife = Instantiate(this.knife, player.transform.position + tempOffset, rotation);
            knife.GetComponent<Rigidbody2D>().velocity = speed * direction;
            yield return new WaitForSeconds(AnimationTimes.instance.ThrowKnivesAnim - knifeDelay);
            player.combatState = Player.CombatState.NonCombat;
            yield return null;
        }

        public int GetCurrentCharge()
        {
            return charge.GetCurrentCharge();
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
    }
}