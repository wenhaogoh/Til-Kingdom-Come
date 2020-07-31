using UnityEngine;

namespace Player
{
    public abstract class Entity : MonoBehaviour
    {
        public int maxHealth;
        protected Health health;
        
        
        // Start is called before the first frame update
        private void Start()
        {
            health = new Health(maxHealth);
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
    }
}
