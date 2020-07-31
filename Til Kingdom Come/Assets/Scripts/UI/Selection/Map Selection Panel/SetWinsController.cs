using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetWinsController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static int wins = 1;
    private int maxWins = 10;
    void Start()
    {
        text.text = wins.ToString();
    }
    public void increaseWins()
    {
        if (wins < maxWins)
        {
            wins++;
            text.text = wins.ToString();
        }
    }
    public void decreaseWins()
    {
        if (wins > 1)
        {
            wins--;
            text.text = wins.ToString();
        }
    }
}
