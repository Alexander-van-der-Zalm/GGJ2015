using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour 
{
    private RoomOptions roomOptions;

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

    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom("Game", roomOptions, null);
    }

	// Update is called once per frame
	void Update () 
    {
	
	}
}
