using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer.Lobby
{
    public class RoomListEntry : MonoBehaviour
    {
        public TextMeshProUGUI RoomNameText;
        public TextMeshProUGUI RoomPlayersText;
        public Button JoinRoomButton;

        private string roomName;

        public void Start()
        {
            JoinRoomButton.onClick.AddListener(() =>
            {
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }

                PhotonNetwork.JoinRoom(roomName);
            });
        }

        public void Initialize(string name, byte currentPlayers, byte maxPlayers)
        {
            roomName = name;

            RoomNameText.text = name;
            RoomPlayersText.text = "Players: " + currentPlayers + "/" + maxPlayers;
        }
    }
}