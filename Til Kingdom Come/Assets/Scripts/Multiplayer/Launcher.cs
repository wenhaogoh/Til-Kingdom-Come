using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Multiplayer.Lobby;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Multiplayer
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private PhotonView pv;
        public PanelsController panelsController;
        public SceneLoaderController sceneLoaderController;
        [Header("Login Panel")]
        public TMP_InputField nicknameInput;

        [Header("Lobby Panel")]
        public GameObject roomListContent;
        public GameObject roomListEntryPrefab;

        [Header("Room Panel")]
        public TextMeshProUGUI playerOneName;
        public TextMeshProUGUI playerTwoName;
        public GameObject toSkillSelectionButton;

        [Header("Skill Selection Panel")]
        public SkillSelectionController skillSelectionController;
        public GameObject startGameButton;
        public GameObject readyButton;
        public TextMeshProUGUI readyButtonText;
        private bool isReady;

        private Dictionary<string, RoomInfo> cachedRoomList;
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            pv = GetComponent<PhotonView>();
            cachedRoomList = new Dictionary<string, RoomInfo>();
            nicknameInput.text = PlayerPrefs.GetString("Nickname", "");
        }
        
        #region Login Panel
        public void OnLoginButtonClicked()
        {
            string nickname = nicknameInput.text;
            PlayerPrefs.SetString("Nickname", nickname);
            if (nickname.Length > 0)
            {
                PhotonNetwork.LocalPlayer.NickName = nickname;
                if (PhotonNetwork.IsConnected)
                {
                    panelsController.SetPanelActive("Selection Panel");
                }
                else
                {
                    PhotonNetwork.ConnectUsingSettings();
                }
            }
            else
            {
                // Throw error message?
                Debug.LogError("Nickname is invalid.");
            }
        }
        public override void OnConnectedToMaster()
        {
            panelsController.SetPanelActive("Selection Panel");
        }
        #endregion

        #region Selection Panel
        public void ToMapSelectionButtonClicked()
        {
            panelsController.SetPanelActive("Map Selection Panel");
        }
        public void OnSelectionPanelBackButtonClicked()
        {
            if (!PhotonNetwork.IsConnected)
            {
                panelsController.SetPanelActive("Login Panel");
            }
            PhotonNetwork.Disconnect();
        }
        public void OnJoinRoomButtonClicked()
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
            panelsController.SetPanelActive("Lobby Panel");
        }
        #endregion

        #region Map Selection Panel
        public void OnCreateRoomButtonClicked()
        {
            string roomName = PlayerPrefs.GetString("Nickname", "") + "'s room";

            var roomOptions = new RoomOptions();

            Hashtable properties = new Hashtable() {{"wins", SetWinsController.GetWins()}, {"map", SetMapController.GetMap()}};
            roomOptions.CustomRoomProperties = properties;
            roomOptions.CustomRoomPropertiesForLobby = new string[] {"wins", "map"};

            roomOptions.MaxPlayers = 2;

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
        #endregion

        #region Room Panel
        public void OnRoomPanelBackButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }
        public void OnRoomPanelNextButtonClicked()
        {
            photonView.RPC("OnRoomPanelNextButtonClickedRPC", RpcTarget.All);
        }
        [PunRPC]
        private void OnRoomPanelNextButtonClickedRPC()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                skillSelectionController.EnableMultiplayerMode(true);
                startGameButton.SetActive(true);
                startGameButton.GetComponent<Button>().interactable = false;
                readyButton.SetActive(false);
            }
            else
            {
                skillSelectionController.EnableMultiplayerMode(false);
                startGameButton.SetActive(false);
                readyButton.SetActive(true);
                isReady = false;
            }
            panelsController.SetPanelActive("Skill Selection Panel");
        }
        #endregion

        #region Lobby Panel
        public void OnLobbyPanelBackButtonClicked()
        {
            panelsController.SetPanelActive("Selection Panel");
        }

        private void ClearRoomListView()
        {
            foreach (Transform roomListEntry in roomListContent.transform)
            {
                GameObject.Destroy(roomListEntry.gameObject);
            }
        }
        
        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                    }
                    continue;
                }

                // Update cached room info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }
            }
        }
        private void UpdateRoomListView()
        {
            foreach (RoomInfo info in cachedRoomList.Values)
            {
                string roomName = info.Name;
                int playerCount = info.PlayerCount;
                int maxPlayers = info.MaxPlayers;
                var wins = info.CustomProperties["wins"];
                var map = info.CustomProperties["map"];
                GameObject roomListEntry = Instantiate(roomListEntryPrefab);
                roomListEntry.transform.SetParent(roomListContent.transform);
                roomListEntry.GetComponent<RoomListEntry>().Initialize(info.Name, info.PlayerCount, (int) info.MaxPlayers, (int) info.CustomProperties["wins"], (int) info.CustomProperties["map"]);
            }
        }
        #endregion

        #region Skill Selection Panel
        public void OnReadyButtonClicked()
        {
            isReady = !isReady;
            photonView.RPC("OnReadyButtonClickedRPC", RpcTarget.All, isReady);
        }
        [PunRPC]
        private void OnReadyButtonClickedRPC(bool isReady)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                startGameButton.GetComponent<Button>().interactable = isReady;
            }
            else
            {
                if (isReady)
                {
                    readyButtonText.text = "Ready!";
                }
                else
                {
                    readyButtonText.text = "Ready?";
                }
            }
        }
        public void OnStartGameButtonClicked()
        {
            photonView.RPC("OnStartGameButtonRPC", RpcTarget.All);
        }
        [PunRPC]
        private void OnStartGameButtonClickedRPC()
        {
            sceneLoaderController.LoadScene("Arena");
        }
        #endregion

        public override void OnDisconnected(DisconnectCause cause)
        {
            panelsController.SetPanelActive("Login Panel");
        }
        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerOneName.text = PhotonNetwork.PlayerList[0].NickName;
                playerTwoName.text = "";
                toSkillSelectionButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                playerOneName.text = PhotonNetwork.PlayerList[0].NickName;
                playerTwoName.text = PhotonNetwork.PlayerList[1].NickName;
                SetMapController.SetMap((int) PhotonNetwork.CurrentRoom.CustomProperties["map"]);
                SetWinsController.SetWins((int) PhotonNetwork.CurrentRoom.CustomProperties["wins"]);
                toSkillSelectionButton.SetActive(false);
            }
            panelsController.SetPanelActive("Room Panel");
        }

        public override void OnLeftRoom()
        {
            panelsController.SetPanelActive("Selection Panel");
            playerOneName.text = "";
            playerTwoName.text = "";
            toSkillSelectionButton.SetActive(true);
        }
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            playerOneName.text = playerTwoName.text;
            playerTwoName.text = "";
            toSkillSelectionButton.GetComponent<Button>().interactable = false;
        }
        
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            playerTwoName.text = newPlayer.NickName;
            toSkillSelectionButton.GetComponent<Button>().interactable = true;
        }
        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                toSkillSelectionButton.SetActive(true);
                toSkillSelectionButton.GetComponent<Button>().interactable = false;
            }
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearRoomListView();
            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }
    }
}
