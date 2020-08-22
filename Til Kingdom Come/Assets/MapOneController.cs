using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MapOneController : MonoBehaviour
{
    private float frequencyOfBoulderSpawn = 0.005f;
    private float boulderSpawnYAxis = 25;
    public GameObject boulder;
    public GameObject playerOnePanel, playerTwoPanel;

    void Start()
    {
        AudioController.instance.PlayMusic("Map 1");
    }
    private void Update()
    {
        if (!playerOnePanel.activeSelf || !playerTwoPanel.activeSelf)
        {
            float randomNumber = Random.Range(0, 1f);
            if (randomNumber < frequencyOfBoulderSpawn)
            {
                float boulderSpawnXAxis = Random.Range(-15, 15);
                var boulderSpawnPosition = new Vector3(boulderSpawnXAxis, boulderSpawnYAxis, 0);
                if (SceneManager.GetActiveScene().name == "Arena")
                    Instantiate(boulder, boulderSpawnPosition, Quaternion.identity);
                else
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.InstantiateSceneObject("Boulder", boulderSpawnPosition, Quaternion.identity);
                    }
                }
            }
        }
    }
}
