using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundStartPanelController : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI text;
    private float speed = 800f;
    private float freezeDuration = 1f;
    private Vector3 hiddenPosition;
    private Vector3 targetPosition = Vector3.zero;
    public bool lower;
    private bool raise;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        hiddenPosition = (Vector3) rectTransform.anchoredPosition;
        lower = false;
        raise = false;
    }
    void Update()
    {
        float step = speed * Time.deltaTime;
        if (lower)
        {
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetPosition, step);
            if ((Vector3) rectTransform.anchoredPosition == targetPosition)
            {
                lower = false;
                StartCoroutine(Freeze(freezeDuration));
            }
        }
        if (raise)
        {
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, hiddenPosition, step);
            if ((Vector3) rectTransform.anchoredPosition == hiddenPosition)
            {
                raise = false;
            }
        }
    }
    public void Trigger()
    {
        lower = true;
    }
    private IEnumerator Freeze(float duration)
    {
        yield return new WaitForSeconds(duration);
        raise = true;
    }
}
