using System.Collections;
using System.Collections.Generic;
using Player_Scripts;
using Player_Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUiController : MonoBehaviour
{
    private const int NUMBEROFSKILLS = 4;
    public Player player;
    public ICooldown[] cooldowns = new ICooldown[NUMBEROFSKILLS];
    public Image[] spriteIcons = new Image[NUMBEROFSKILLS];
    public Image[] darkMasks = new Image[NUMBEROFSKILLS];

    private void Start()
    {
        // Assign icons for each skill
        for(int i = 0; i < NUMBEROFSKILLS; i++)
        {
            cooldowns[i] = player.skills[i];
            spriteIcons[i].sprite = cooldowns[i].GetIcon();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < NUMBEROFSKILLS; i++)
        {
            ICooldown cooldown = cooldowns[i];
            float nextAvailableTime = cooldown.GetNextAvailableTime();
            float cooldownDuration = cooldown.GetCooldownDuration();
            if (nextAvailableTime < Time.time)
            {
                return;
            }
            else
            {
                darkMasks[i].fillAmount = Mathf.Lerp(0f, 1f, (nextAvailableTime - Time.time) / cooldownDuration);
            }
        }
    }
}
