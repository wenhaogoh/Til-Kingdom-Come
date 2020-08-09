using System.Collections.Generic;
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
        private PhotonView photonView;
        
        [Header("Login Panel")]
        public GameObject LoginPanel;
        public TextMeshProUGUI PlayerNameInput;

        [Header("Selection Panel")]
        public GameObject SelectionPanel;

        [Header("Create Room Panel")]
        public GameObject CreateRoomPanel;

        [Header("Lobby Panel")]
        public GameObject RoomListPanel;
        public GameObject RoomListContent;
        public GameObject RoomListEntryPrefab;

        [Header("Room Panel")]
        public GameObject RoomLobbyPanel;
        public Transform LobbyHorizontalLayoutGroup;
        public GameObject PlayerEntryPrefab;
        public GameObject startButton;

        [Header("Skill Select Panel")] 
        public GameObject SkillSelectPanel;
        public GameObject ReadyButton;
        public TextMeshProUGUI ReadyText;
        public Button skillSelectStartButton;

        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;
        private Dictionary<int, GameObject> playerListEntries;
        


        #region Private Fields

        private bool isConnecting;
        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";
        
        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            
            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();
            photonView = GetComponent<PhotonView>();
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            PlayerNameInput.text = PlayerPrefs.GetString("Nickname", "");
        }

        #endregion

        


        #region UI CALLBACKS

        public void OnRoomLobbyBackButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void OnNicknamePanelBackButtonClicked()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void OnSelectionPanelBackButtonClicked()
        {
            if (!PhotonNetwork.IsConnected)
            {
                SetActivePanel(LoginPanel.name);
            }
            PhotonNetwork.Disconnect();
        }
        
        public void OnCreateRoomButtonClicked()
        {
            string roomName = PlayerPrefs.GetString("Nickname", "") + "'s room";
            var roomOptions = new RoomOptions {MaxPlayers = 2};
            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }

        #endregion

        public void SetActivePanel(string activePanel)
        {
            LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
            SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
            CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
            RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
            RoomLobbyPanel.SetActive(activePanel.Equals(RoomLobbyPanel.name));
            SkillSelectPanel.SetActive(activePanel.Equals(SkillSelectPanel.name));
        }

        public void OnRoomListButtonClicked()
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }

            SetActivePanel(RoomListPanel.name);
        }
        
        public void OnLoginButtonClicked()
        {
            string playerName = PlayerNameInput.text;
            PlayerPrefs.SetString("Nickname", playerName);
            
            if (!playerName.Equals(""))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                if (PhotonNetwork.IsConnected)
                {
                    SetActivePanel(SelectionPanel.name);
                }
                else
                {
                    PhotonNetwork.ConnectUsingSettings();
                }
            }
            else
            {
                Debug.LogError("Player Name is invalid.");
            }
        }

        public void OnStartButtonClicked()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("PhotonNetwork: Trying to load a level but we are not the master client");
                return;
            }
            // SetWinsController.photonView.RPC("MultiplayerPassWins", RpcTarget.All, MapChanger.current + 1, UpdateWins.wins);
            photonView.RPC("SkillSelectRPC", RpcTarget.All);
            ReadyButton.SetActive(!PhotonNetwork.IsMasterClient);

            // skillSelectStartButton.interactable = false;
        }

        [PunRPC]
        private void SkillSelectRPC()
        {
            SetActivePanel(SkillSelectPanel.name);
            skillSelectStartButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        }

        public void OnSkillSelectStartButtonClicked()
        {
            PhotonNetwork.LoadLevel("MultiplayerArena");
        }

        public void OnReadyButtonClicked()
        {
            photonView.RPC("RPCReadyButton", RpcTarget.All);
        }

        [PunRPC]
        private void RPCReadyButton()
        {
            Debug.Log("Sending ready button rpc");
            bool isActive = skillSelectStartButton.gameObject.GetComponent<Button>().interactable;
            if (PhotonNetwork.IsMasterClient)
            {
                skillSelectStartButton.gameObject.GetComponent<Button>().interactable = !isActive;
            }
            photonView.RPC("RPCReadyText", RpcTarget.All, !isActive);
        }

        [PunRPC]
        private void RPCReadyText(bool ready)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                if (ready)
                {
                    ReadyText.text = "Ready!";
                }
                else
                {
                    ReadyText.text = "Ready?";
                }
            }
        }

        #region PUN CALLBACKS

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                startButton.gameObject.SetActive(true);
            }
        }
        
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log("Player " + otherPlayer.ActorNumber + " has left the room");
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);
            startButton.GetComponent<Button>().interactable = false;
        }

        public override void OnLeftRoom()
        {
            SetActivePanel(SelectionPanel.name);
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }
            foreach (GameObject entry in playerListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            playerListEntries.Clear();
            playerListEntries = null;
        }
        
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            /*if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }*/
            SetActivePanel(SelectionPanel.name);

        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            SetActivePanel(LoginPanel.name);
        }
        
        public override void OnJoinedRoom()
        {
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerEntryPrefab);
                entry.transform.SetParent(LobbyHorizontalLayoutGroup);
                entry.transform.localScale = Vector3.one;
                // entry.GetComponent<PlayerEntry>().SetName(player.NickName);
                playerListEntries.Add(player.ActorNumber, entry);
            }
            Debug.Log("Joined room");
            SetActivePanel(RoomLobbyPanel.name);
            if (!PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(false);
            }
        }
        
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            GameObject entry = Instantiate(PlayerEntryPrefab);
            entry.transform.SetParent(LobbyHorizontalLayoutGroup.transform);
            entry.transform.localScale = Vector3.one;
            // entry.GetComponent<PlayerEntry>().SetName(newPlayer.NickName);
            playerListEntries.Add(newPlayer.ActorNumber, entry);
            startButton.GetComponent<Button>().interactable = true;
        }
        
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearRoomListView();

            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }


        public override void OnCreatedRoom()
        {
            Debug.Log("Room created");
        }

        #endregion
        
        private void ClearRoomListView()
        {
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
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
                GameObject entry = Instantiate(RoomListEntryPrefab);
                entry.transform.SetParent(RoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

                roomListEntries.Add(info.Name, entry);
            }
        }

    }
}
