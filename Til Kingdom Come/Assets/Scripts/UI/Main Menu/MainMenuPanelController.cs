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
        mainMenuButtons.transform.localScale = oneVector;
        playMenuButtons.transform.localScale = zeroVector;
        settingsMenuButtons.transform.localScale = zeroVector;
    }
    public void LaunchPlayMenu()
    {
        playMenuButtons.transform.localScale = oneVector;
        mainMenuButtons.transform.localScale = zeroVector;
        settingsMenuButtons.transform.localScale = zeroVector;
    }
    public void LaunchSettingsMenu()
    {
        settingsMenuButtons.transform.localScale = oneVector;
        playMenuButtons.transform.localScale = zeroVector;
        mainMenuButtons.transform.localScale = zeroVector;
    }
}
