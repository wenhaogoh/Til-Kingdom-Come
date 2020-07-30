using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScenePanelsController : MonoBehaviour
{
    public GameObject mainMenuPanel, volumePanel, controlsPanel;
    private Vector3 zeroVector = new Vector3(0, 0, 0);
    private Vector3 oneVector = new Vector3(1, 1, 1);
    public void LaunchMainMenuPanel()
    {
        volumePanel.transform.localScale = zeroVector;
        controlsPanel.transform.localScale = zeroVector;
        mainMenuPanel.transform.localScale = oneVector;
    }
    public void LaunchVolumePanel()
    {
        mainMenuPanel.transform.localScale = zeroVector;
        volumePanel.transform.localScale = oneVector;
    }
    public void LaunchControlsPanel()
    {
        mainMenuPanel.transform.localScale = zeroVector;
        controlsPanel.transform.localScale = oneVector;
    }
}
