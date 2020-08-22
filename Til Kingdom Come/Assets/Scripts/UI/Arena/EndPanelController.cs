using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPanelController : MonoBehaviour
{
    public GameObject playerOneVictory, playerTwoVictory;
    private RectTransform rectTransform;
    private float speed = 800f;
    private Vector3 targetPosition = Vector3.zero;
    private bool lower;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        lower = false;
    }
    private void Update()
    {
        if (lower)
        {
            float step = speed * Time.deltaTime;
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetPosition, step);
            if ((Vector3) rectTransform.anchoredPosition == targetPosition)
            {
                lower = false;
            }
        }
    }
    public void Trigger(int playerNo)
    {
        AudioController.instance.PlaySoundEffect("Victory");
        if (playerNo == 1)
        {
            playerOneVictory.SetActive(true);
        }
        else if (playerNo == 2)
        {
            playerTwoVictory.SetActive(true);
        }
        lower = true;
    }
}
