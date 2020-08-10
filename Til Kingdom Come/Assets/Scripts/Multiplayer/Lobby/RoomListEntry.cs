using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer.Lobby
{
    public class RoomListEntry : MonoBehaviour
    {
        public TextMeshProUGUI roomName;
        public TextMeshProUGUI playerCount;
        public TextMeshProUGUI maxPlayers;
        public TextMeshProUGUI wins;
        public TextMeshProUGUI map;
        public Button joinRoom;
        public void Initialize(string roomName, int playerCount, int maxPlayers, int wins, int map)
        {
            this.roomName.text = roomName;
            this.playerCount.text = playerCount.ToString();
            this.maxPlayers.text = maxPlayers.ToString();
            this.wins.text = wins.ToString();
            this.map.text = map.ToString();

            joinRoom.onClick.AddListener(() =>
            {
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }
                PhotonNetwork.JoinRoom(roomName);
            });
        }
    }
}