using UnityEngine;

namespace UI
{
    public class SelectionMenuPanelController : MonoBehaviour
    {
        public GameObject SkillSelectionPanel;
        public GameObject MapSelectionPanel;

        public void LaunchMapSelection()
        {
            SkillSelectionPanel.transform.localScale = Vector3.zero;
            MapSelectionPanel.transform.localScale = Vector3.one;
        }

        public void LaunchSkillSelection()
        {
            SkillSelectionPanel.transform.localScale = Vector3.one;
            MapSelectionPanel.transform.localScale = Vector3.zero;
        }
    }
}
