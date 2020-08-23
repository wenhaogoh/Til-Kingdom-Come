using UnityEngine;
using Random = UnityEngine.Random;

namespace Arena.Boulder
{
    public class BoulderController : MonoBehaviour
    {
        public float size;

        private void Awake()
        {
            GameManager.onPlayerDeath += DestroyBoulder;
        }

        // Start is called before the first frame update
        private void Start()
        {
            float randomSize = Random.Range(1, 3);
            size = randomSize;
            transform.localScale = new Vector3(randomSize, randomSize, 1);
        }

        private void DestroyBoulder()
        {
            Destroy(gameObject);
        }

        private void DestroyBoulder(int i)
        {
            DestroyBoulder();
        }

        private void OnDestroy()
        {
            GameManager.onPlayerDeath -= DestroyBoulder;
        }
    }
}
