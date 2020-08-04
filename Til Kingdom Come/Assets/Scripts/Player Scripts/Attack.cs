using System;
using System.Collections;
using UnityEngine;

namespace Player_Scripts
{
    public class Attack : Skill, ICharge
    {
        private Charge charge;
        private int maxCharge = 3;
        private float chargeTime = 4f;

        private void Start()
        {
            name = "Attack";
            cooldown = chargeTime;
            charge = new Charge(maxCharge, chargeTime);
        }

        private void Update()
        {
            Charging();
        }

        public override void Cast(Player player)
        { 
            if (charge.GetCurrentCharge() <= 0)
            {
                return;
            }
            player.anim.SetTrigger(name);
            if (charge.IsFullyCharged())
            {
                nextAvailableTime = Time.time + chargeTime;
            }
            charge.DecreaseCharge();
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
