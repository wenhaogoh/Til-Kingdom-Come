namespace Player_Scripts
{
    public class Health
    {
        private int maxHealth;
        private int currentHealth;

        public Health(int maxHealth)
        {
            this.maxHealth = maxHealth;
            this.currentHealth = maxHealth;
        }
        
        public void DecreaseHealth(int amount)
        {
            currentHealth -= amount;
        }
        
        public void IncreaseHealth(int amount)
        {
            currentHealth += amount;
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
        }
    }
}