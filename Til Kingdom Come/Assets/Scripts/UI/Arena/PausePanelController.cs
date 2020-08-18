using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player_Scripts;
using UI.Arena;

public class PausePanelController : MonoBehaviour
{
    public GameObject pauseMenu, blurEffect;
    private bool gameIsPaused;
    private bool canPause;
    private void Awake()
    {
        gameIsPaused = false;
        canPause = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            if(gameIsPaused)
            {
                if (GameManager.IsOnline())
                {

                }
                else
                {
                    Resume();
                }
            }
            else
            {
                if (GameManager.IsOnline())
                {
                    
                }
                else
                {
                    Pause();
                }
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
    public void Resume()
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
    public void EnablePause()
    {
        canPause = true;
    }
}
