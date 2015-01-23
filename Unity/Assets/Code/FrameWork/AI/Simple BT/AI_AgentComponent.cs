using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AI_AgentComponent : MonoBehaviour 
{

    public AI_Agent Agent;

    public AI_BlackboardComponent GlobalBlackboardC;
    public AI_BlackboardComponent LocalBlackboardC;

	// Use this for initialization
	void Start () 
    {
        if (LocalBlackboardC != null)
            Agent.LocalBlackboard = LocalBlackboardC.Blackboard;
        if (GlobalBlackboardC != null)
            Agent.GlobalBlackboard = GlobalBlackboardC.Blackboard;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
