using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_Scripts;

public class PausePanelController : MonoBehaviour
{
    public GameObject pauseMenu, blurEffect;
    private bool gameIsPaused;
    private bool canPause;
    private void Awake()
    {
        gameIsPaused = false;
        canPause = true;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    private void Pause()
    {
        PlayerInput.onDisableInput.Invoke();
        AudioController.instance.PauseCurrentMusic();
        pauseMenu.SetActive(true);
        blurEffect.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    private void Resume()
    {
        PlayerInput.onEnableInput.Invoke();
        AudioController.instance.PlayCurrentMusic();
        pauseMenu.SetActive(false);
        blurEffect.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    public void DisablePause()
    {
        canPause = false;
    }
}
