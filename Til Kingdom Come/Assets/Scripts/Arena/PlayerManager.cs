﻿using Cinemachine;
using Player_Scripts;
using Player_Scripts.Interfaces;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private float spawnDistance = 10f;
    private float yOffset = -4f;
    public CinemachineTargetGroup group;
    public CooldownUiController playerOneCooldownUi;
    public CooldownUiController playerTwoCooldownUi;
    public HealthBarController playerOneHealthBar;
    public HealthBarController playerTwoHealthBar;

    public GameObject AttackPrefab;


    // Start is called before the first frame update
    private void Awake()
    {
        var playerOneGameObject = Instantiate(playerPrefab, new Vector3(-spawnDistance, yOffset, 0), Quaternion.identity);
        var playerTwoGameObject = Instantiate(playerPrefab, new Vector3(spawnDistance, yOffset, 0), Quaternion.Euler(0, 180, 0));
        
        PlayerSetUp(playerOneGameObject, playerTwoGameObject);
    }

    private void PlayerSetUp(GameObject playerOneGameObject, GameObject playerTwoGameObject)
    {
        // Add players to camera target group
        @group.AddMember(playerOneGameObject.transform, 1, 0);
        @group.AddMember(playerTwoGameObject.transform, 1, 0);

        Player playerOne = playerOneGameObject.GetComponent<Player>();
        Player playerTwo = playerTwoGameObject.GetComponent<Player>();

        // Adding skills to players
        var playerOneAttack = Instantiate(AttackPrefab, Vector3.zero, Quaternion.identity);
        playerOne.AddSkill(playerOneAttack.GetComponent<Skill>());
        var playerTwoAttack = Instantiate(AttackPrefab, Vector3.zero, Quaternion.identity);
        playerTwo.AddSkill(playerTwoAttack.GetComponent<Skill>());
        
        playerOneCooldownUi.player = playerOne.GetComponent<Player>();
        playerTwoCooldownUi.player = playerTwo.GetComponent<Player>();

        var playerOneChargeControllers = playerOneCooldownUi.GetComponentsInChildren<ChargeController>();
        var playerTwoChargeControllers = playerTwoCooldownUi.GetComponentsInChildren<ChargeController>();

        SetCharge(playerOne, playerOneChargeControllers);
        SetCharge(playerTwo, playerTwoChargeControllers);

        playerOneHealthBar.entity = playerOne.GetComponent<IHealthBar>();
        playerTwoHealthBar.entity = playerTwo.GetComponent<IHealthBar>();
        
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
}
