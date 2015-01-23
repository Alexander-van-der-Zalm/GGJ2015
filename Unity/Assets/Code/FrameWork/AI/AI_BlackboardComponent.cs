using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AI_BlackboardComponent : MonoBehaviour 
{
    public AI_Blackboard Blackboard
    {
        get { return blackBoard == null ? blackBoard = AI_Blackboard.Create() : blackBoard; }
        set { blackBoard = value; }
    }

    [SerializeField]
    private AI_Blackboard blackBoard;
}
