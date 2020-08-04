﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarCloudsController : MonoBehaviour
{
    private RectTransform rectTransform;
    private float speed = -25f;
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
