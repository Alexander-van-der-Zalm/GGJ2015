using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class NetworkManager : MonoBehaviour
{
    #region Fields

    public bool OfflineMode = false;

    private RoomOptions roomOptions;
    private GameManagement mgr;
    private bool createdRoom = false;

    private PhotonView photonView;

    #endregion

    #region Start & GUI label

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Use this for initialization
	void OnEnable () 
    {
        if (!OfflineMode)
        {
            PhotonNetwork.ConnectUsingSettings("0.1");
            Debug.Log("Connecting");
            roomOptions = new RoomOptions() { maxPlayers = 2, isVisible = true, isOpen = true };
            Debug.Log(PhotonNetwork.player.ID);
        }
        else
        {
            PhotonNetwork.offlineMode = true;
            Debug.Log(PhotonNetwork.player);
            UnitManager.Instance.RespawnAllUnits();
        }
	}

    void OnGUI()
    {
        if (!OfflineMode)
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else
        {
            GUILayout.Label("OfflineMode");
        }
    }

    #endregion

    #region JoinLobby

    void OnJoinedLobby()
    {
        // Set mgr
        mgr = GetComponent<GameManagement>() as GameManagement;

        // Join random room
        PhotonNetwork.JoinRandomRoom();
            
        // Player 2

        // 
    }

    #endregion

    public void LoadLevel()
    {

    }

    public void StartMatch()
    {

    }


    // Client
    // (X) Join room
    // (X) Set Player info
    // (X) Recieve Level info
    // (X) Clear all spawns
    // (X) Send & Start countdown when level recieved



    // Host
    // (X) Create room
    // (X) Set Player info
    // (X) Clear all spawns
    // (X) Send Level info
    // (X) Start countdown when level recieved

    void OnJoinedRoom()
    {
        Debug.Log("Joined the room id: " +  PhotonNetwork.player.ID);

        // Set Player info
        if(createdRoom)
        { // Player 1
            Debug.Log("Player1");
            UnitManager.Instance.team = 0;
        }
        else
        { // Player 2
            Debug.Log("Player2");
            UnitManager.Instance.team = 2;
            photonView.RPC("RequestLevelName", PhotonTargets.Others);
        }

        // Change this to start game when both players are ready
        
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room! Creating a new one - Player1");
        PhotonNetwork.CreateRoom("Game", roomOptions, null);
        // Player 1
    }

    void OnCreatedRoom ()
    {
        // Player 1
        // Room creation stuff
        createdRoom = true;

        // Set server info
        Debug.Log("OnCreatedRoom");       
    }

    [RPC]
    private void RequestLevelName()
    {
        Debug.Log("Request level data RPC");
        photonView.RPC("SetLevelName", PhotonTargets.All, mgr.BlockMgr.SelectedLevel);

        UnitManager.Instance.RespawnAllUnits();
    }

    [RPC]
    private void SetLevelName(string levelName)
    {
        Debug.Log("Load Level RPC: " + levelName);
        
        // ADD some GUI stuff during loading
        //mgr.BlockPrefab = blockPrefab;
        mgr.BlockMgr.loadLevel(levelName);
    }
}
