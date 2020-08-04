using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButtonController : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float speed = 200f;
    private bool pointerEnter;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        targetPosition = startPosition + new Vector3(70 ,0 , 0);
    }
    private void Update()
    {
        float step = speed * Time.deltaTime;
        if (pointerEnter)
        {
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetPosition, step);
        }
        else
        {
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, startPosition, step);
        }
    }
    public void PopIn()
    {
        pointerEnter = true;
    }
    public void PopOut()
    {
        pointerEnter = false;
    }
}
