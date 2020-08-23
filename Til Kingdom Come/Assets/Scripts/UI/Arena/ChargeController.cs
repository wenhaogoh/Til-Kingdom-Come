using Player_Scripts.Interfaces;
using TMPro;
using UnityEngine;

namespace UI.Arena
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