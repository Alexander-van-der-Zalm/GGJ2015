using ExitGames.Client.Photon.LoadBalancing;
 
public class NetworkManager
{
    private LoadBalancingClient client;
 

    private void OnEventHandler(byte eventCode, object content, PhotonPlayer sender) { Debug.Log("OnEventHandler"); }
    

    // this is called when the client loaded and is ready to start   
    void Start()
    {
        client = new LoadBalancingClient();
        client.AppId = "1cff2ce2-292a-4f4c-9cc0-5d26d993d0eb";  // edit this!
 
        // "eu" is the European region's token
        bool connectInProcess = client.ConnectToRegionMaster("eu");  // can return false for errors

        bool host = true;

        if (host = true) {
            peer.OpCreateRoom("Room", 2, customGameProperties, propertiesListedInLobby);
        }

        PhotonNetwork.OnEventCall += this.OnEventHandler;
    }

    void joinRoom() {

        // join random rooms easily, filtering for specific room properties, if needed
        Hashtable expectedCustomRoomProperties = new Hashtable();
        expectedCustomRoomProperties["map"] = 1; // custom props can have any name but the key must be string
        peer.OpJoinRandomRoom(expectedCustomRoomProperties, (byte)expectedMaxPlayers);
   
    } 

    void sendMessage(){
        byte eventCode = 1; // make up event codes at will
        Hashtable evData = new Hashtable(); // put your data into a key-value hashtable
        bool sendReliable = false; // send something reliable if it has to arrive everywhere
        byte channelId = 0; // for advanced sequencing. can be 0 in most cases
        peer.OpRaiseEvent(eventCode, evData, sendReliable, channelId);
    }

    void Update()
    {
        client.Service();
    }
 
    void OnApplicationQuit()
    {
        client.Disconnect();
    }


}