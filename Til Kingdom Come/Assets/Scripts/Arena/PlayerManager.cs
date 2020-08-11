using System.Collections;
using System.Collections.Generic;
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
                PlayerSetUp(playerOneGameObject, 1);
                StartCoroutine(FindPlayerDelay(2));
            }
            else
            {
                var playerTwoGameObject = PhotonNetwork.Instantiate("Player", new Vector3(spawnDistance, yOffset, 0), Quaternion.Euler(0, 180, 0));
                playerTwoGameObject.transform.parent = transform;
                playerTwoGameObject.name = "Player 2";
                PlayerSetUp(playerTwoGameObject, 2);
                var playerOneGameObject = GameObject.Find("Player(Clone)");
                StartCoroutine(FindPlayerDelay(1));
            }
        }
        else
        {
            var playerOneGameObject = Instantiate(playerPrefab,
                transform.position + new Vector3(-spawnDistance, yOffset, 0), Quaternion.identity);
            var playerTwoGameObject = Instantiate(playerPrefab, new Vector3(spawnDistance, yOffset, 0),
                Quaternion.Euler(0, 180, 0));

            PlayerSetUp(playerOneGameObject, 1);
            PlayerSetUp(playerTwoGameObject, 2);
        }
    }

    private void PlayerSetUp(GameObject playerGameObject, int playerNo)
    {
        group.AddMember(playerGameObject.transform, 1, 0);

        Player player = playerGameObject.GetComponent<Player>();
        player.SetPlayerNo(playerNo);
        players.Add(player);
        
        // Adding skills to players
        AddBasicSkills(player);
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

    private IEnumerator FindPlayerDelay(int playerNo)
    {
        yield return new WaitForSeconds(1f);
        var playerTwoGameObject = GameObject.Find("Player(Clone)");
        PlayerSetUp(playerTwoGameObject, playerNo);
        yield return null;
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
