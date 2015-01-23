using UnityEngine;
using System.Collections;

public interface ICharacterAction 
{
    bool Interuptable { get; }
    bool CanInterupt { get; }
    bool Finished { get; }
    bool CannotMoveDuringAction { get; }
    string Name { get; }

    /// <summary>
    /// Start the action (could start a coroutine)
    /// </summary>
    void StartAction(AnimatorCollectionWrapper animator);

    /// <summary>
    /// Also possible to call if it is not interruptable (do an interuptable check if you dont want that to happen).
    /// </summary>
    void StopAction();
}
