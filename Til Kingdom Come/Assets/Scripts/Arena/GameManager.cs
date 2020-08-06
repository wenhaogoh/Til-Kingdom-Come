using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public List<GameObject> maps;
    public TextMeshProUGUI playerOneScore, playerTwoScore;
    private int map;
    private int wins;
    private void Awake()
    {
        map = SetMapController.GetMap();
        maps[map].SetActive(true);

        wins = SetWinsController.GetWins();

        playerOneScore.text = "0";
        playerTwoScore.text = "0";
    }
}