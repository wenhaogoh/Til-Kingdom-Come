using UnityEngine;

namespace Player_Scripts
{
    public class Combo
    {
        public enum ComboNumber
        {
            One,
            Two,
            Three
        }

        private ComboNumber currentCombo = ComboNumber.One;
        public ComboNumber CurrentCombo => currentCombo;
        private float decayTime = 3f;
        private float nextDecayTime;

        public void UpdateCombo()
        {
            // after an attack, updates what the next combo will be
            switch (currentCombo)
            {
                case ComboNumber.One:
                    currentCombo = ComboNumber.Two;
                    break;
                case ComboNumber.Two:
                    currentCombo = ComboNumber.Three;
                    break;
                case ComboNumber.Three:
                    currentCombo = ComboNumber.One;
                    break;
            }
        }

        public void SetDecay()
        {
            nextDecayTime = Time.time + decayTime;
        }

        public void UpdateDecay()
        {
            // able to chain attack within a certain window, else performs a normal attack
            switch (currentCombo)
            {
                case ComboNumber.One:
                    return;
                case ComboNumber.Two:
                    if (Time.time > nextDecayTime) currentCombo = ComboNumber.One;
                    break;
                case ComboNumber.Three:
                    if (Time.time > nextDecayTime) currentCombo = ComboNumber.One;
                    break;
            }
        }
    }
}