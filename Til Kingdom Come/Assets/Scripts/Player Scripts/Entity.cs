using Player_Scripts.Interfaces;
using UnityEngine;

namespace Player_Scripts
{
    public abstract class Entity : MonoBehaviour, IHealthBar
    {
        private int maxHealth;
        private int currentHealth;

        protected void DecreaseHealth(int amount)
        {
            currentHealth -= amount;
        }

        public void IncreaseHealth(int amount)
        {
            currentHealth += amount;
        }

        protected bool IsDead()
        {
            return currentHealth <= 0;
        }

        protected void SetMaxHealth(int maxHealth)
        {
            this.maxHealth = maxHealth;
        }

        protected void RefillCurrentHealth()
        {
            this.currentHealth = maxHealth;
        }

        public float GetHealthRatio()
        {
            return (float) currentHealth / (float) maxHealth;
        }

        public float GetHealth()
        {
            return currentHealth;
        }
    }
    
}
