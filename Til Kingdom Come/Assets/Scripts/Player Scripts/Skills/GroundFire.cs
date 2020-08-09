using UnityEngine;

namespace Player_Scripts.Skills
{
    public class GroundFire : MonoBehaviour
    {
        private BoxCollider2D boxCollider2D;
        private float fireDuration = 5f;
        private int damagePerTick = 5;
        private float timeBetweenTicks = 0.5f;
        private float nextTime;
        private float endTime;

        // Start is called before the first frame update
        private void Awake()
        {
            nextTime = Time.time;
            endTime = Time.time + fireDuration;
            boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (Time.time > endTime)
            {
                boxCollider2D.enabled = false;
            }
            
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<Player>();
            // next time the player can be damaged
            if (nextTime < Time.time)
            {
                player.TakeDamage(damagePerTick);
                nextTime = Time.time + timeBetweenTicks;
            }
        }
    }
}