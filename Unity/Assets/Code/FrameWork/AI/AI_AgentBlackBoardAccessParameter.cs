using UnityEngine;
using System.Collections;

[System.Serializable]
public struct AI_AgentParameter
{
    public AI_Agent.BlackBoard AgentAccesType;
    
    public string ParameterName;

    public GUIContent Content;

    public AI_AgentParameter(string parametername, AI_Agent.BlackBoard access, GUIContent content)
    {
        Content = content;
        ParameterName = parametername;
        AgentAccesType = access;
    }

    public AI_AgentParameter(string parametername = "", AI_Agent.BlackBoard access = AI_Agent.BlackBoard.local)
        : this(parametername, access, null)
    { }
}
