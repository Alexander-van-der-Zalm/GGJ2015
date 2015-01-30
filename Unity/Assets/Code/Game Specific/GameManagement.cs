using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BlockManager)), RequireComponent(typeof(UnitManager)),
RequireComponent(typeof(NetworkManager)), RequireComponent(typeof(InputManager)), 
RequireComponent(typeof(SelectionManager))]
public class GameManagement : Singleton<GameManagement>
{
    #region Fields

    [HideInInspector]
    public BlockManager BlockMgr;
    [HideInInspector]
    public UnitManager UnitMgr;
    [HideInInspector]
    public NetworkManager NetworkMgr;
    [HideInInspector]
    public InputManager InputMgr;
    [HideInInspector]
    public SelectionManager SelectionMgr;
    [HideInInspector]
    public Rules RulesMgr;
    [HideInInspector]
    public PhotonView PhotonView;

    #endregion

    #region Static Properties

    public static BlockManager Block { get { return Instance.BlockMgr; } }
    public static UnitManager Unit { get { return Instance.UnitMgr; } }
    public static NetworkManager Network { get { return Instance.NetworkMgr; } }
    public static InputManager Input { get { return Instance.InputMgr; } }
    public static SelectionManager Selection { get { return Instance.SelectionMgr; } }
    public static Rules Rules { get { return Instance.RulesMgr; } }
    public static PhotonView Photon { get { return Instance.PhotonView; } }
    #endregion

    #region Awake

    // Use this for initialization
	void Awake () 
    {        
        BlockMgr = GetComponent<BlockManager>();
        UnitMgr = GetComponent<UnitManager>();
        NetworkMgr = GetComponent<NetworkManager>();
        InputMgr = GetComponent<InputManager>();
        SelectionMgr = GetComponent<SelectionManager>();
        RulesMgr = GetComponent<Rules>();
        PhotonView = GetComponent<PhotonView>();
    }

    #endregion
}
