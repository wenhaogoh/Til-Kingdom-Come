using UnityEngine;

namespace UI.Main_Menu.Clouds
{
    public class NearCloudsController : MonoBehaviour
    {
        private RectTransform rectTransform;
        private float speed = -40f;
        private float sizeOfMap = 1920;
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        private void Update()
        {
            if (rectTransform.anchoredPosition.x <= -sizeOfMap) {
                rectTransform.anchoredPosition = new Vector2(sizeOfMap, rectTransform.anchoredPosition.y);
            }
        
            rectTransform.anchoredPosition += new Vector2(Time.deltaTime * speed, 0);
        }
    }
}
