using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour 
{
    [System.Serializable]
    public class ServerInfo
    {
        //Think what is needed

        // Level (for loading)
        // SpawnedUnits
        
        // Waiting for other player
    }
    
    private RoomOptions roomOptions;
    private BlockManager mgr;
    private bool createdRoom = false;

    public PhotonView PhotonView { get { return GetComponent<PhotonView>(); } }

    #region Start & GUI label

    // Use this for initialization
	void Start () 
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
        roomOptions = new RoomOptions() { maxPlayers = 2, isVisible = true, isOpen = true };
	}

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    #endregion

    #region JoinLobby

    void OnJoinedLobby()
    {
        // Set mgr
        mgr = BlockManager.Instance;

        // Join random room
        PhotonNetwork.JoinRandomRoom();
        // Player 2

        // 
    }

    #endregion


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
        Debug.Log("Joined the room");

        // Set Player info
        if(createdRoom)
        { // Player 1
            UnitManager.Instance.team = 0;
        }
        else
        { // Player 2
            UnitManager.Instance.team = 2;
        }

    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room! Player1");
        PhotonNetwork.CreateRoom("Game", roomOptions, null);
        // Player 1

        
    }

    void OnCreatedRoom ()
    {
        //// Player 1
        //// Room creation stuff
        //createdRoom = true;

        //// Set server info
        //Debug.Log("OnCreatedRoom");

        //// Load selected level
        //BlockListSO blocks = BlockManager.Instance.GetLevelData(mgr.SelectedLevel);
        //GameObject blockPrefab = BlockManager.Instance.BlockPrefab;

        //SendLevelData(blocks, blockPrefab);
    }

    private void SendLevelData(BlockListSO blocks, GameObject blockPrefab)
    {
        PhotonView.RPC("SetLevelData", PhotonTargets.All, blocks, blockPrefab);
        Debug.Log("Send level data RPC");
    }

    [RPC]
    private void SetLevelData(BlockListSO blocks, GameObject blockPrefab)
    {
        Debug.Log("Level: " + blocks.name + " - block: " + blockPrefab.name);
        
        // ADD some GUI stuff during loading
        mgr.BlockPrefab = blockPrefab;
        mgr.LoadBlocks(blocks);
       
    }
}
