using TMPro;
using UnityEngine;

namespace UI.Selection.Map_Selection_Panel
{
    public class SetWinsController : MonoBehaviour
    {
        public TextMeshProUGUI text;
        private static int wins = 1;
        private int maxWins = 10;
        public static int GetWins()
        {
            return wins;
        }
        public static void SetWins(int i)
        {
            wins = i;
        }
        private void Start()
        {
            text.text = wins.ToString();
        }
        public void increaseWins()
        {
            if (wins < maxWins)
            {
                wins++;
                text.text = wins.ToString();
            }
        }
        public void decreaseWins()
        {
            if (wins > 1)
            {
                wins--;
                text.text = wins.ToString();
            }
        }
    }
}
