using Player_Scripts;
using Player_Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Arena
{
    public class CooldownUiController : MonoBehaviour
    {
        private const int NUMBEROFSKILLS = 4;
        public Player player;
        public ICooldown[] cooldowns = new ICooldown[NUMBEROFSKILLS];
        public Image[] spriteIcons = new Image[NUMBEROFSKILLS];
        public Image[] darkMasks = new Image[NUMBEROFSKILLS];

        private void Start()
        {
            if (!GameManager.IsOnline())
            {
                AssignIcons(player);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            for (int i = 0; i < NUMBEROFSKILLS; i++)
            {
                if (cooldowns[i] == null)
                {
                    continue;
                }
                // Updates sprite icons for combos
                spriteIcons[i].sprite = cooldowns[i].GetIcon();
                ICooldown cooldown = cooldowns[i];
                float nextAvailableTime = cooldown.GetNextAvailableTime();
                float cooldownDuration = cooldown.GetCooldownDuration();
                if (nextAvailableTime < Time.time) // Skill is available and ready to use
                {
                    darkMasks[i].fillAmount = 0;
                }
                else
                {
                    darkMasks[i].fillAmount = Mathf.Lerp(0f, 1f, (nextAvailableTime - Time.time) / cooldownDuration);
                }
            }
        }

        public void AssignIcons(Player player)
        {
            // Assign icons for each skill
            for(int i = 0; i < NUMBEROFSKILLS; i++)
            {
                if (player.skills[i] == null)
                {
                    continue;
                }
                cooldowns[i] = player.skills[i];
            }
        }
    }
}
