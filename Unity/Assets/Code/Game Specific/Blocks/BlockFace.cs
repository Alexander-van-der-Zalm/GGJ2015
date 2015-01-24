using UnityEngine;
using System.Collections;

// Face Info?
// Has Unit
// Captured
// CapturedState?

[ExecuteInEditMode]
public class BlockFace : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private int id;

    public bool HasUnit;

    private Transform tr;

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    // Change this to mesh rotation
    public Quaternion Rotation 
    { 
        get 
        {
            return Quaternion.LookRotation(Normal);
        } 
    }

    public Vector3 Normal
    {
       get
       {
            Vector3 center = transform.parent.transform.position;
            Vector3 normalDirection = transform.position - center;
            return normalDirection.normalized;
       }
        
    }

    #endregion

    #region ClickEvent

    public void OnMouseDown()
    {
        Debug.Log("CLicked " + ID);
        //SelectionManager Stuff

    }

    #endregion

    #region Gizmos

    public void OnDrawGizmosSelected ()
    {
        Gizmos.color = new Color(0.1f, 0.8f, 0.1f, 0.3f);

        // Rotate towards normal
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, Rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawCube(Vector3.zero, new Vector3(.15f,.15f,.25f));
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.1f, 0.1f, 0.1f, 0.3f);
        Gizmos.DrawSphere(transform.position, .15f);
    }

    #endregion
}
