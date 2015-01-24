using UnityEngine;
using System.Collections;

public class Camera_Rotation : MonoBehaviour {

	public Transform target;
	public float speed = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton (1)){
			this.cameraMove ();
		}	
		this.zoomIn ();

	}

	private void cameraLook(){
		Vector3 lookRot =  target.position - this.transform.position;
		this.transform.rotation = Quaternion.LookRotation (lookRot);
	}

	private void cameraMove(){
		this.transform.RotateAround(target.position, transform.up, Input.GetAxis("Mouse X")*speed);
		this.transform.RotateAround(target.position, -transform.right, Input.GetAxis("Mouse Y")*speed);
	}

	private float mouseSpeedX(){
		return Input.GetAxis ("Mouse X") / Time.deltaTime;
	}

	//private float decrementSpeed(float speed){
	//	
	//}

	private void zoomIn(){
		this.camera.orthographicSize -= Input.GetAxis ("Mouse ScrollWheel");
	}
}
