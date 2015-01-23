using UnityEngine;
using System.Collections;

public interface ICharacter2DMovement  
{
    //float MaximumVelocity { get; set; }
    
    /// <summary>
    /// Initiate the movement of the character
    /// </summary>
    /// <param name="horizontalInput"></param>
    /// <param name="verticalInput"></param>
    /// <returns>Succes if can move</returns>
    void SetMovementInput(float horizontalInput, float verticalInput);

    void FixedPhysicsUpdate(Rigidbody2D rb);
}
