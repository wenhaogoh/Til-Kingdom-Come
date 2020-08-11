﻿using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using Player_Scripts;
using Player_Scripts.Interfaces;
using UI.Arena;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private float spawnDistance = 10f;
    private float yOffset = -5f;
    public CinemachineTargetGroup group;
    public CooldownUiController playerOneCooldownUi;
    public CooldownUiController playerTwoCooldownUi;
    public HealthBarController playerOneHealthBar;
    public HealthBarController playerTwoHealthBar;

    public GameObject[] basicSkills = new GameObject[3];
    public List<GameObject> selectedSkills;

    public List<Player> players = new List<Player>();


    // Start is called before the first frame update
    private void Awake()
    {
        if (GameManager.IsMultiplayer())
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var playerOneGameObject = PhotonNetwork.Instantiate("Player", transform.position + new Vector3(-spawnDistance, yOffset, 0), Quaternion.identity);
                playerOneGameObject.transform.parent = transform;
                playerOneGameObject.name = "Player 1";
                PlayerSetUp(playerOneGameObject);
                var otherPlayer = GameObject.Find("Player(Clone)");
                PlayerSetUp(otherPlayer);

            }
            else
            {
                var playerTwoGameObject = PhotonNetwork.Instantiate("Player", new Vector3(spawnDistance, yOffset, 0), Quaternion.Euler(0, 180, 0));
                playerTwoGameObject.transform.parent = transform;
                playerTwoGameObject.name = "Player 2";
                PlayerSetUp(playerTwoGameObject);
                var otherPlayer = GameObject.Find("Player(Clone)");
                PlayerSetUp(otherPlayer);

            }
        }
        else
        {
            var playerOneGameObject = Instantiate(playerPrefab,
                transform.position + new Vector3(-spawnDistance, yOffset, 0), Quaternion.identity);
            var playerTwoGameObject = Instantiate(playerPrefab, new Vector3(spawnDistance, yOffset, 0),
                Quaternion.Euler(0, 180, 0));

            PlayerSetUp(playerOneGameObject, playerTwoGameObject);
        }
    }

    private void PlayerSetUp(GameObject playerOneGameObject, GameObject playerTwoGameObject)
    {
        // Add players to camera target group
        group.AddMember(playerOneGameObject.transform, 1, 0);
        group.AddMember(playerTwoGameObject.transform, 1, 0);

        Player playerOne = playerOneGameObject.GetComponent<Player>();
        Player playerTwo = playerTwoGameObject.GetComponent<Player>();

        players.Add(playerOne);
        players.Add(playerTwo);

        // Adding skills to players
        AddBasicSkills(playerOne);
        AddBasicSkills(playerTwo);
        playerOne.AddSkill(selectedSkills[SkillSelectionController.GetPlayerOneSkill()]);
        playerTwo.AddSkill(selectedSkills[SkillSelectionController.GetPlayerTwoSkill()]);
        
        playerOneCooldownUi.player = playerOne.GetComponent<Player>();
        playerTwoCooldownUi.player = playerTwo.GetComponent<Player>();

        var playerOneChargeControllers = playerOneCooldownUi.GetComponentsInChildren<ChargeController>();
        var playerTwoChargeControllers = playerTwoCooldownUi.GetComponentsInChildren<ChargeController>();

        SetCharge(playerOne, playerOneChargeControllers);
        SetCharge(playerTwo, playerTwoChargeControllers);

        playerOneHealthBar.entity = playerOne.GetComponent<IHealthBar>();
        playerTwoHealthBar.entity = playerTwo.GetComponent<IHealthBar>();
    }

    private void PlayerSetUp(GameObject playerGameObject)
    {
        group.AddMember(playerGameObject.transform, 1, 0);

        Player player = playerGameObject.GetComponent<Player>();
        players.Add(player);
        
        // Adding skills to players
        AddBasicSkills(player);
        var playerNo = player.GetPlayerNo();
        if (playerNo == 1)
        {
            player.AddSkill(selectedSkills[SkillSelectionController.GetPlayerOneSkill()]);
            playerOneCooldownUi.player = player.GetComponent<Player>();
            var playerOneChargeControllers = playerOneCooldownUi.GetComponentsInChildren<ChargeController>();
            SetCharge(player, playerOneChargeControllers);
            playerOneHealthBar.entity = player.GetComponent<IHealthBar>();
            
        }
        else
        {
            player.AddSkill(selectedSkills[SkillSelectionController.GetPlayerTwoSkill()]);
            playerTwoCooldownUi.player = player.GetComponent<Player>();
            var playerTwoChargeControllers = playerTwoCooldownUi.GetComponentsInChildren<ChargeController>();
            SetCharge(player, playerTwoChargeControllers);
            playerTwoHealthBar.entity = player.GetComponent<IHealthBar>();
        }
    }

    private void AddBasicSkills(Player player)
    {
        foreach (var basicSkill in basicSkills)
        {
            if (basicSkill == null) continue;
            player.AddSkill(basicSkill);
        }
    }

    private void SetCharge(Player player, ChargeController[] chargeController)
    {
        for (int i = 0; i < 4; i++)
        {
            var skill = player.skills[i];
            if (skill == null)
            {
                return;
            }
            if (skill.GetComponent<ICharge>() != null)
            {
                chargeController[i].charge = skill.GetComponent<ICharge>();
            }
            else
            {
                chargeController[i].text.enabled = false;
            }
        }
    }

    public void ResetAllPlayers()
    {
        foreach (var player in players)
        {
            player.ResetPlayer();
        }
    }

    public void EnableInvulnerabilityForAllPlayers()
    {
        foreach (var player in players)
        {
            player.enableInvulnerability();
        }
    }
}
