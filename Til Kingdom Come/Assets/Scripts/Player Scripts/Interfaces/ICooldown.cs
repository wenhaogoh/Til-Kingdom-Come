using UnityEngine;

namespace Player_Scripts.Interfaces
{
    public interface ICooldown
    {
        float GetNextAvailableTime();

        float GetCooldownDuration();

        Sprite GetIcon();
    }
}
