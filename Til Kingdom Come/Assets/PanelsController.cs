using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsController : MonoBehaviour
{
    public GameObject[] panels;
    public void SetPanelActive(string panelName)
    {
        foreach (GameObject panel in panels)
        {
            if (panel.name == panelName)
            {
                panel.transform.localScale = Vector3.one;
            }
            else
            {
                panel.transform.localScale = Vector3.zero;
            }
        }
    }
}
