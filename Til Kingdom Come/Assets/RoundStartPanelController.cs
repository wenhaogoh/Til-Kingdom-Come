using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Player_Scripts;

public class RoundStartPanelController : MonoBehaviour
{
    private RectTransform rectTransform;
    public TextMeshProUGUI roundNumber;
    private float speed = 800f;
    private float freezeDuration = 1f;
    private Vector3 hiddenPosition;
    private Vector3 targetPosition = Vector3.zero;
    private bool lower;
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
        if (lower)
        {
            float step = speed * Time.deltaTime;
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetPosition, step);
            if ((Vector3) rectTransform.anchoredPosition == targetPosition)
            {
                lower = false;
                StartCoroutine(Freeze(freezeDuration));
            }
        }
        if (raise)
        {
            float step = speed * Time.deltaTime;
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, hiddenPosition, step);
            if ((Vector3) rectTransform.anchoredPosition == hiddenPosition)
            {
                raise = false;
                PlayerInput.onEnableInput.Invoke();
            }
        }
    }
    public void Trigger(int roundNumber)
    {
        this.roundNumber.text = roundNumber.ToString();
        lower = true;
    }
    private IEnumerator Freeze(float duration)
    {
        yield return new WaitForSeconds(duration);
        raise = true;
    }
}
