using TMPro;
using UnityEngine;

namespace Online.Lobby
{
    public class PlayerEntry : MonoBehaviour
    {
        public TextMeshProUGUI playerName;
        public void SetName(string name)
        {
            playerName.text = name;
        }
        
    }
}