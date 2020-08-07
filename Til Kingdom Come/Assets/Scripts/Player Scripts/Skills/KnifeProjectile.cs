using UnityEngine;

namespace Player_Scripts.Skills
{
    public class KnifeProjectile : MonoBehaviour
    {
        public GameObject sparks;
        
        private int damage = 20;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // destroys projectile if it touches a wall
            if (collision.CompareTag("Wall")) DestroyProjectile();
            if (collision.CompareTag("Projectile"))
            {
                Instantiate(sparks, transform.position, Quaternion.identity);
                DestroyProjectile();
            }
            
            if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            // projectile collides a player
            Player damagedPlayer = collision.GetComponent<Player>();
            if (damagedPlayer == null) return;
            switch (damagedPlayer.combatState)
            {
                // Player cant be hit while rolling
                case Player.CombatState.Rolling:
                    return;
                // Player will not be damaged while blocking (projectile is destroyed)
                case Player.CombatState.Blocking:
                    DestroyProjectile();
                    Instantiate(sparks, transform.position, Quaternion.identity);
                    break;
                default:
                    damagedPlayer.TakeDamage(damage);
                    DestroyProjectile();
                    return;
            }
        }
        
        private void DestroyProjectile()
        {
            Destroy(gameObject);    
        }
    }
}