using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderController : MonoBehaviour
{
    public float size;
    private void Awake()
    {

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
}
