using TMPro;
using UnityEngine;

namespace Multiplayer.Lobby
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