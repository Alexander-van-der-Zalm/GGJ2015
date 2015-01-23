using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Guarantees a moveable controller with the possibility to perform actions
/// One active action max per ICharacterActionController
/// </summary>
public interface ICharacterActionController 
{
    bool CanMove { get; }

    List<string> GetDefaultActionNames { get; }

    bool HasActiveAction { get; }

	// Becomes one action max
    ICharacterAction ActiveAction { get; }

    /// <summary>
    /// Try let the character controller do an action
    /// </summary>
    /// <typeparam name="T">A characterAction with ICharacterAction</typeparam>
    /// <returns>Returns if the action has started</returns>
    bool StartAction<T>(AnimatorCollectionWrapper anim) where T : ICharacterAction;


    /// <summary>
    /// Try let the character controller do an action
    /// </summary>
    bool StartAction(ICharacterAction action,AnimatorCollectionWrapper anim);

    /// <summary>
    /// Stop active actin if it has the generic type (even if uninteruptable)
    /// </summary>
    /// <typeparam name="T">Action type to stop : ICharacterAction</typeparam>
    /// <returns>Did the action exist and stop?</returns>
    bool StopAction<T>() where T : ICharacterAction;

    /// <summary>
    /// Used to stop and remove a specific active action. Always succeeds if the selected action exists (even if uninteruptable).
    /// </summary>
    /// <param name="action">Action instance to stop</param>
    /// <returns>Did the action exist and stop?</returns>
    bool StopAction(ICharacterAction action);

    /// <summary>
    /// Stops the active action
    /// </summary>
    /// <param name="overrideUninteruptables">If true even stops if the actions are flagged as uninteruptable</param>
    /// <returns>Succes of stopping the action</returns>
    bool StopAction(bool overrideUninteruptable);
}
