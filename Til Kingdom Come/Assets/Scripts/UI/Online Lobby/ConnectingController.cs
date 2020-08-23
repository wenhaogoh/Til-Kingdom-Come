using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.Online_Lobby
{
    public class ConnectingController : MonoBehaviour
    {
        public TextMeshProUGUI text;
        private float interval = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Animation(interval));
        }

        private IEnumerator Animation(float interval)
        {
            while (true)
            {
                text.text = "Connecting";
                yield return new WaitForSeconds(interval);
                text.text = "Connecting.";
                yield return new WaitForSeconds(interval);
                text.text = "Connecting..";
                yield return new WaitForSeconds(interval);
                text.text = "Connecting...";
                yield return new WaitForSeconds(interval);
            }
        }

        public void SetActive()
        {
            transform.localScale = Vector3.one;
        }

        public void SetInactive()
        {
            transform.localScale = Vector3.zero;
        }
    }
}
