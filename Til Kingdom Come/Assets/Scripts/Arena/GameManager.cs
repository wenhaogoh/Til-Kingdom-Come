using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public List<GameObject> maps;
    public TextMeshProUGUI playerOneScoreText, playerTwoScoreText;
    public RoundStartPanelController roundStartPanel;
    public EndPanelController endPanel;
    public static Action<int> onPlayerDeath;
    private int map, wins, playerOneScore, playerTwoScore;
    private void Awake()
    {
        map = SetMapController.GetMap();
        maps[map].SetActive(true);

        wins = SetWinsController.GetWins();
        playerOneScore = 0;
        playerTwoScore = 0;

        playerOneScoreText.text = "0";
        playerTwoScoreText.text = "0";

        onPlayerDeath += PlayerDeathEvent;
    }
    private void Start()
    {
        int roundNumber = playerOneScore + playerTwoScore + 1;
        roundStartPanel.Trigger(roundNumber);
    }
    private void PlayerDeathEvent(int playerNo)
    {
        // update scores
        if (playerNo == 1)
        {
            // player one dies
            playerTwoScore++;
            playerTwoScoreText.text = playerTwoScore.ToString();
        }
        else if (playerNo == 2)
        {
            // player two dies
            playerOneScore++;
            playerOneScoreText.text = playerOneScore.ToString();
        }
        
        // trigger subsequent event
        if (playerOneScore >= wins)
        {
            PlayerWinEvent(1);
        }
        else if (playerTwoScore >= wins)
        {
            PlayerWinEvent(2);
        }
        else
        {
            NextRoundEvent();
        }
    }

    private void NextRoundEvent()
    {
        int roundNumber = playerOneScore + playerTwoScore + 1;
        roundStartPanel.Trigger(roundNumber);
    }

    private void PlayerWinEvent(int playerNo)
    {
        endPanel.Trigger(playerNo);
    }
}