using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Player_Scripts;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public CinemachineTargetGroup group;
    public CooldownUiController playerOneCooldownUi;
    public CooldownUiController playerTwoCooldownUi;


    // Start is called before the first frame update
    private void Awake()
    {
        var playerOne = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        var playerTwo = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        group.AddMember(playerOne.transform, 1, 0);
        group.AddMember(playerTwo.transform, 1, 0);
        playerOneCooldownUi.player = playerOne.GetComponent<Player>();
        playerTwoCooldownUi.player = playerTwo.GetComponent<Player>();
        var playerOneChargeControllers = playerOneCooldownUi.GetComponentsInChildren<ChargeController>();
        var playerTwoChargeControllers = playerTwoCooldownUi.GetComponentsInChildren<ChargeController>();
        for (int i = 0; i < 4; i++)
        {
            var skill = playerOne.GetComponent<Player>().skills[i];
            if (skill.GetComponent<ICharge>() != null)
            {
                playerOneChargeControllers[i].charge = skill.GetComponent<ICharge>();
            }
            else
            {
                playerOneChargeControllers[i].text.enabled = false;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
