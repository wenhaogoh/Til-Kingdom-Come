using Audio;
using Player_Scripts;
using UnityEngine;

namespace Arena.Boulder
{
    public class BoulderProjectileController : MonoBehaviour
    {
        public static int soundEffectClipNumber = 1;
        private int totalSoundEffectClips = 3;
        private float damage = 10; // ranges from 10 to 30 based on size
        public GameObject collideEffect;
        public float heightOffset;
        public GameObject parent;
        private float size;

        private void Start()
        {
            size = parent.GetComponent<BoulderController>().size;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // destroys projectile if it touches a wall
            if (collision.CompareTag("Wall")) Impact();
            
            // projectile collides a player
            Player damagedPlayer = collision.GetComponent<Player>();
            if (damagedPlayer == null)
            {
                Debug.Log("no player fouind");
                return;
            }
            switch (damagedPlayer.combatState)
            {
                // Player cant be hit while rolling
                case Player.CombatState.Rolling:
                    return;
                case Player.CombatState.Dead:
                    return;
                default:
                    damagedPlayer.TakeDamage((int) (damage * size));
                    Impact();
                    return;
            }
        }
        public void Impact()
        {
            // Alternate between clips to prevent sound from being cut off
            AudioController.instance.PlaySoundEffect("Ground Smash " + soundEffectClipNumber);
            if (soundEffectClipNumber == totalSoundEffectClips)
            {
                soundEffectClipNumber = 1;
            }
            else
            {
                soundEffectClipNumber++;
            }
            var offSet = new Vector3(0, heightOffset, 0);
            Instantiate(collideEffect, transform.position + offSet, Quaternion.identity);
            Destroy(parent);
        }
    }
}
