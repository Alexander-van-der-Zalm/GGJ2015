using UnityEngine;
using System.Collections;

// Face Info?
// Has Unit
// Captured
// CapturedState?

public class BlockFace : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private int id;

    public bool HasUnit;

    #endregion

    #region Properties

    public int ID { get { return id; } set { id = value; } }

    public Quaternion Rotation { get { return transform.localRotation; } }

    #endregion

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
        //Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, transform.rotation,Vector3.one);
        //Gizmos.matrix = rotationMatrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, new Vector3(.15f,.25f,.15f));
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.1f, 0.1f, 0.1f, 0.3f);
        Gizmos.DrawSphere(transform.position, .15f);
    }

    #endregion
}
