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

    private bool createdRoom = false;

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
        PhotonNetwork.JoinRandomRoom();
        // Player 2

        // 
    }

    #endregion


    // Client
    // Join room
    // Set Player info
    // Recieve Level info
    // Send countdown when level recieved

    // Host
    // Create room
    // Set Player info

    void OnJoinedRoom()
    {
        Debug.Log("Joined the room");

        // Set Player info
        if(createdRoom)
        {
            //UnitManager.Instance.
        }
        else
        {

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
        // Player 1
        // Room creation stuff
        createdRoom = true;

        // Set server info
        Debug.Log("OnCreatedRoom");
        // 
    }
}
