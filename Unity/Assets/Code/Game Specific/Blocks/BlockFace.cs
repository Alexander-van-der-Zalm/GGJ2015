using UnityEngine;
using System.Collections;

// Face Info?
// Has Unit
// Captured
// CapturedState?

public class BlockFace : MonoBehaviour
{
   [SerializeField]
    private int id;

    public int ID { get { return id; } set { id = value; } }

    #region ClickEvent

    public void Click()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Gizmos

    public void OnDrawGizmosSelected ()
    {
        Gizmos.color = new Color(0.1f, 0.8f, 0.1f, 0.3f);
        Gizmos.DrawSphere(transform.position, 0.25f);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.1f, 0.1f, 0.1f, 0.3f);
        Gizmos.DrawSphere(transform.position, 0.25f);
    }

    #endregion
}
