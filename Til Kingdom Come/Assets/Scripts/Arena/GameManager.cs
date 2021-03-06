﻿using System;
using System.Collections;
using System.Collections.Generic;
using Player_Scripts;
using TMPro;
using UI.Arena;
using UI.Selection.Map_Selection_Panel;
using UnityEngine;

namespace Arena
{
    public class GameManager : MonoBehaviour
    {
        public PlayerManager playerManager;
        public List<GameObject> maps;
        public TextMeshProUGUI playerOneScoreText, playerTwoScoreText, targetScore;
        public RoundStartPanelController roundStartPanel;
        public EndPanelController endPanel;
        public PausePanelController pausePanel;
        public static Action<int> onPlayerDeath;
        private int map, wins, playerOneScore, playerTwoScore;
        private static bool _onlineMode;

        private void Awake()
        {
            map = SetMapController.GetMap();
            maps[map].SetActive(true);

            wins = SetWinsController.GetWins();
            targetScore.text = wins.ToString();
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
                StartCoroutine(PlayerWinEvent(1));
            }
            else if (playerTwoScore >= wins)
            {
                StartCoroutine(PlayerWinEvent(2));
            }
            else
            {
                StartCoroutine(NextRoundEvent());
            }
        }

        private IEnumerator NextRoundEvent()
        {
            playerManager.EnableInvulnerabilityForAllPlayers();
            yield return new WaitForSeconds(AnimationTimes.instance.DeathAnim + 0.5f);
            playerManager.ResetAllPlayers();
            int roundNumber = playerOneScore + playerTwoScore + 1;
            roundStartPanel.Trigger(roundNumber);
            yield return null;
        }

        private IEnumerator PlayerWinEvent(int playerNo)
        {
            playerManager.EnableInvulnerabilityForAllPlayers();
            yield return new WaitForSeconds(AnimationTimes.instance.DeathAnim + 0.5f);
            endPanel.Trigger(playerNo);
            pausePanel.DisablePause();
            yield return null;
        }

        public static bool IsOnline()
        {
            return _onlineMode;
        }

        public static void SetOnlineMode(bool onlineBool)
        {
            _onlineMode = onlineBool;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            onPlayerDeath -= PlayerDeathEvent;
        }
    }
}