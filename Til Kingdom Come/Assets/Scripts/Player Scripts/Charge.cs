namespace Player_Scripts
{
    public class Charge
    {
        private int maxCharge;
        private int currentCharge;
        private float chargeTime;

        public Charge(int maxCharge, float chargeTime)
        {
            this.maxCharge = maxCharge;
            currentCharge = maxCharge;
            this.chargeTime = chargeTime;
        }

        public bool IsFullyCharged()
        {
            return currentCharge == maxCharge;
        }

        public void IncreaseCharge()
        {
            if (currentCharge < maxCharge)
            {
                currentCharge++;
            }
        }

        public void DecreaseCharge()
        {
            if (currentCharge > 0)
            {
                currentCharge--;
            }
        }

        public int GetCurrentCharge()
        {
            return currentCharge;
        }

        public float GetChargeTime()
        {
            return chargeTime;
        }

        public bool IsChargeEmpty()
        {
            return currentCharge <= 0;
        }
    
    }
}
