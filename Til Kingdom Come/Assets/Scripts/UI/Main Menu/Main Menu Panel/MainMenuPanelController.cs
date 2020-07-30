using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanelController : MonoBehaviour
{
    public GameObject mainMenuButtons;
    public GameObject playMenuButtons;
    public GameObject settingsMenuButtons;
    private Vector3 zeroVector = new Vector3(0, 0, 0);
    private Vector3 oneVector = new Vector3(1, 1, 1);
    public void LaunchMainMenu()
    {
        playMenuButtons.transform.localScale = zeroVector;
        settingsMenuButtons.transform.localScale = zeroVector;
        mainMenuButtons.transform.localScale = oneVector;
    }
    public void LaunchPlayMenu()
    {
        mainMenuButtons.transform.localScale = zeroVector;
        settingsMenuButtons.transform.localScale = zeroVector;
        playMenuButtons.transform.localScale = oneVector;
    }
    public void LaunchSettingsMenu()
    {
        playMenuButtons.transform.localScale = zeroVector;
        mainMenuButtons.transform.localScale = zeroVector;
        settingsMenuButtons.transform.localScale = oneVector;
    }
}
