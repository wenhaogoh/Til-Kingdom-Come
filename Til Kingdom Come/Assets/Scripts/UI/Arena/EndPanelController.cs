using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPanelController : MonoBehaviour
{
    public GameObject playerOneVictory, playerTwoVictory;
    private RectTransform rectTransform;
    private float speed = 800f;
    private Vector3 targetPosition = Vector3.zero;
    private bool lower;
    private PhotonView pv;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        pv = GetComponent<PhotonView>();
        lower = false;
    }
    private void Update()
    {
        if (lower)
        {
            float step = speed * Time.deltaTime;
            rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetPosition, step);
            if ((Vector3) rectTransform.anchoredPosition == targetPosition)
            {
                lower = false;
            }
        }
    }
    public void Trigger(int playerNo)
    {
        AudioController.instance.PlaySoundEffect("Victory");
        if (playerNo == 1)
        {
            playerOneVictory.SetActive(true);
        }
        else if (playerNo == 2)
        {
            playerTwoVictory.SetActive(true);
        }
        lower = true;
    }

    public void OnHomeButtonClicked()
    {
        if (GameManager.IsOnline())
        {
            pv.RPC("RPCHome", RpcTarget.All);
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    [PunRPC]
    private void RPCHome()
    {
        PhotonNetwork.LeaveRoom();
        StartCoroutine(SceneChange());
    }
    
    private IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Online Lobby");
        yield return null;
    }

    public void OnRestartButtonClicked()
    {
        if (GameManager.IsOnline())
        {
            // Reloads scene on all clients since PhotonNetwork.LoadLevel cannot be used
            pv.RPC("RPCRestart", RpcTarget.All);
        }
        else
        {
            SceneManager.LoadScene("Arena");
        }
    }

    [PunRPC]
    private void RPCRestart()
    {
        PhotonNetwork.LoadLevel("Arena");
    }
}
