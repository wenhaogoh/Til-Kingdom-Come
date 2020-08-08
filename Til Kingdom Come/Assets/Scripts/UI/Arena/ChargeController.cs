using TMPro;
using UnityEngine;

namespace Player_Scripts
{
    public class ChargeController : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public ICharge charge;

        private void Update()
        {
            if (charge == null)
            {
                return;
            }
            text.text = charge.GetCurrentCharge().ToString();
        }
    }
}