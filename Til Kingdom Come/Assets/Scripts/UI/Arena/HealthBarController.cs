using Player_Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Arena
{
    public class HealthBarController : MonoBehaviour
    {
        public IHealthBar entity;
        public Image healthBarFill;

        private void Update()
        {
            healthBarFill.fillAmount = entity.GetHealthRatio();
        }
    }
}
