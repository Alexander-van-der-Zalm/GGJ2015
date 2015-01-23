using UnityEngine;
using System.Collections;

[System.Serializable,ExecuteInEditMode]
public class AnimationSwap2Selector : MonoBehaviour 
{
    [SerializeField]
    public AnimationSwap2Collection CollectionReference;

    [SerializeField]
    public Animator Head;

    [SerializeField]
    public Animator Body;

    [SerializeField]
    public int HeadVariety;

    [SerializeField]
    public int BodyVariety;


	// Use this for initialization
	void Start () 
    {
        //CollectionReference.SetAnimator(Head, Body, HeadVariety, BodyVariety);
	}
	
}
