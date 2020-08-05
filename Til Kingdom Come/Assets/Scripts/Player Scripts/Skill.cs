using Player_Scripts.Interfaces;
using UnityEngine;

namespace Player_Scripts
{
    public abstract class Skill : MonoBehaviour, ICooldown
    {
        [SerializeField] protected new string name;
        [SerializeField] protected string info;
        [SerializeField] protected float cooldown;
        protected float nextAvailableTime;

        public Sprite icon;

        public float GetNextAvailableTime()
        {   
            return nextAvailableTime;
        }

        protected bool CanCast()
        {
            if (Time.time < nextAvailableTime) {
                return false;
            } 
            else
            {
                nextAvailableTime = cooldown + Time.time;
                return true;
            }

        }

        public abstract void Cast(Player player);

        public float GetCooldownDuration()
        {
            return cooldown;
        }

        public Sprite GetIcon()
        {
            return icon;
        }
    }
}
