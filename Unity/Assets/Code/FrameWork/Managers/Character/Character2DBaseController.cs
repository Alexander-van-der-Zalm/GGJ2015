using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

// TODOOOO
// V - Think about if and how multiple actions are handled - 1 Action mAx
// V - Finish stopaction
// V - ICharacterController -> IActionController
// V - Base implements an actionController (also possible to have multipe action controllers (run n gun)
// X - Implementation has a list of exposeable actions binded to the metalogic described in the character controllers 
//     to easily make variations (like Jump, run, etc)
// X - Think about how to change movement from action (Fraction multiplier)
// X - Think about how to initiate movement from action (override input?) change movementControllers?
// V - Think about how to prevent Flip from action (expose to action?)
// X - Think about how to expose animation to action (required component of an action?)
// X - Give action a collection of target and caller CharacterAnimation, ActionController and Movement?

/// <summary>
/// Can be inherented from to avoid reimplementing a basic flip
/// </summary>
[System.Serializable, RequireComponent(typeof(Rigidbody2D))]
public class Character2DBaseController : MonoBehaviour
{
    #region Fields

    [SerializeField]
    protected CharacterActionController ActionController;

    [SerializeField]
    protected ICharacter2DMovement MovementController;

    [SerializeField]
    protected AnimatorCollectionWrapper AnimationController;

    [System.NonSerialized]
    protected Rigidbody2D rb;

    [System.NonSerialized]
    private Transform tr;

    [System.NonSerialized]
    private bool facingLeft = true;

    #endregion

    #region Properties

    protected virtual bool CanFlip { get { return true; } }

    #endregion

    #region Start

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        tr = gameObject.GetComponent<Transform>();
        Init();
    }

    protected virtual void Init()
    {
        if (ActionController == null || MovementController == null || AnimationController == null)
            throw new System.NullReferenceException("Character2DBaseController.Init movement, animation or action controller not set");
    }

    #endregion

    #region Fixed physics update

    void FixedUpdate()
    {
        FixedUpdateStart();
        
        if (CanFlip)
            CheckFlipBy(rb.velocity.x);

        if (ActionController.CanMove)
        {
            MovementController.FixedPhysicsUpdate(rb);
        }
        else
            rb.velocity = Vector2.zero;
            

        FixedUpdateEnd();
    }

    protected virtual void FixedUpdateEnd()
    {

    }

    protected virtual void FixedUpdateStart()
    {

    }

    #endregion

    #region SetMovementInput

    public void SetMovementInput(float horizontal, float vertical)
    {
        // Stop action if it is interruptable, prevents movement and if the input is not 0
        if((horizontal != 0 || vertical != 0) && !ActionController.CanMove && !ActionController.Uninterruptable)
        {
            ActionController.StopAction(false);
            Debug.Log("SetMovementInput: Stopping all actions");
        }
        
        MovementController.SetMovementInput(horizontal, vertical);
    }

    #endregion

    #region Flip

    private void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 localScale = tr.localScale;
        localScale.x = -localScale.x;
        tr.localScale = localScale;
    }

    /// <summary>
    /// dir positive for right
    /// dir negative for left
    /// </summary>
    /// <param name="dir"></param>
    protected void CheckFlipBy(float dir)
    {
        if (dir == 0)
            return;
        dir = Mathf.Sign(dir);

        if (dir > 0 && facingLeft)
            Flip();
        else if (dir < 0 && !facingLeft)
            Flip();
    }

    #endregion
}
