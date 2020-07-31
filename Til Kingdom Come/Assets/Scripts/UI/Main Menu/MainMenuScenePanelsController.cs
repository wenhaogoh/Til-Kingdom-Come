using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScenePanelsController : MonoBehaviour
{
    public GameObject mainMenuPanel, volumePanel, controlsPanel;
    public void LaunchMainMenuPanel()
    {
        volumePanel.transform.localScale = Vector3.zero;
        controlsPanel.transform.localScale = Vector3.zero;
        mainMenuPanel.transform.localScale = Vector3.one;
    }
    public void LaunchVolumePanel()
    {
        mainMenuPanel.transform.localScale = Vector3.zero;
        volumePanel.transform.localScale = Vector3.one;
    }
    public void LaunchControlsPanel()
    {
        mainMenuPanel.transform.localScale = Vector3.zero;
        controlsPanel.transform.localScale = Vector3.one;
    }
}
