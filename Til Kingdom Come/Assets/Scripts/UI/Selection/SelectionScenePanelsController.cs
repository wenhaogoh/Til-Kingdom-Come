using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScenePanelsController : MonoBehaviour
{
    public GameObject skillSelectionPanel;
    public GameObject mapSelectionPanel;

    public void LaunchMapSelection()
    {
        skillSelectionPanel.transform.localScale = Vector3.zero;
        mapSelectionPanel.transform.localScale = Vector3.one;
    }

    public void LaunchSkillSelection()
    {
        skillSelectionPanel.transform.localScale = Vector3.one;
        mapSelectionPanel.transform.localScale = Vector3.zero;
    }
}
