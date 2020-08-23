using Audio;
using UnityEngine;

namespace Arena.Maps
{
    public class MapZeroController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            AudioController.instance.PlayMusic("Map 0");
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
