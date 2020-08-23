using UnityEngine;

namespace UI.Selection.Skill_Selection_Panel
{
    public class SkillCellController : MonoBehaviour
    {
        public GameObject p1, p2, p1AndP2;
        public void SelectedByPlayerOne()
        {
            p1AndP2.SetActive(false);
            p2.SetActive(false);
            p1.SetActive(true);
        }
        public void SelectedByPlayerTwo()
        {
            p1AndP2.SetActive(false);
            p1.SetActive(false);
            p2.SetActive(true);
        }
        public void SelectedByBothPlayers()
        {
            p1.SetActive(false);
            p2.SetActive(false);
            p1AndP2.SetActive(true);
        }

        public void NotSelected()
        {
            p1.SetActive(false);
            p2.SetActive(false);
            p1AndP2.SetActive(false);
        }
    }
}
