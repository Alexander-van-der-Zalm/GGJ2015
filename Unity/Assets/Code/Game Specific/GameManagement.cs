using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BlockManager)), RequireComponent(typeof(UnitManager)),
RequireComponent(typeof(NetworkManager)), RequireComponent(typeof(InputManager)), 
RequireComponent(typeof(SelectionManager))]
public class GameManagement : MonoBehaviour 
{    
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

    // Use this for initialization
	void Awake () 
    {        
        BlockMgr = GetComponent<BlockManager>();
        UnitMgr = GetComponent<UnitManager>();
        NetworkMgr = GetComponent<NetworkManager>();
        InputMgr = GetComponent<InputManager>();
        SelectionMgr = GetComponent<SelectionManager>();
	}
}
