using Cinemachine;
using Player_Scripts;
using Player_Scripts.Interfaces;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public CinemachineTargetGroup group;
    public CooldownUiController playerOneCooldownUi;
    public CooldownUiController playerTwoCooldownUi;
    public HealthBarController playerOneHealthBar;
    public HealthBarController playerTwoHealthBar;


    // Start is called before the first frame update
    private void Awake()
    {
        var playerOne = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        var playerTwo = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        
        // Add players to camera target group
        group.AddMember(playerOne.transform, 1, 0);
        group.AddMember(playerTwo.transform, 1, 0);
        
        playerOneCooldownUi.player = playerOne.GetComponent<Player>();
        playerTwoCooldownUi.player = playerTwo.GetComponent<Player>();
        var playerOneChargeControllers = playerOneCooldownUi.GetComponentsInChildren<ChargeController>();
        var playerTwoChargeControllers = playerTwoCooldownUi.GetComponentsInChildren<ChargeController>();

        SetCharge(playerOne, playerOneChargeControllers);
        SetCharge(playerTwo, playerTwoChargeControllers);

        playerOneHealthBar.entity = playerOne.GetComponent<IHealthBar>();
        playerTwoHealthBar.entity = playerTwo.GetComponent<IHealthBar>();

    }

    private void SetCharge(GameObject player, ChargeController[] chargeController)
    {
        for (int i = 0; i < 4; i++)
        {
            var skill = player.GetComponent<Player>().skills[i];
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
