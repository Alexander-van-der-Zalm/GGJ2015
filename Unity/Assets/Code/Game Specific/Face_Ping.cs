using UnityEngine;
using System.Collections;

public class Face_Ping : MonoBehaviour {

	public GameObject prefab;
	public Transform spawnPoint;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			GameObject.Instantiate(prefab, spawnPoint.position, Quaternion.identity);
		}
	}
}
