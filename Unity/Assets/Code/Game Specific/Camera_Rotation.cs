using UnityEngine;
using System.Collections;

public class Camera_Rotation : MonoBehaviour {

	public Transform target;
	public float speed = 5.0f;
	public float mouseSpeedX = 0f;
	public float mouseSpeedY = 0f;

	public Vector3 curentPos;
	public Vector3 targetPos;
	public Vector3 endPos;
	public float T = 0;

	public bool slerping = false;

	public float journeyTime = 2.5F;
	public float startTime;

	void Start() {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton (1)){
			mouseSpeedX = getMouseSpeedX();
			mouseSpeedY = getMouseSpeedY();
		}	
		this.cameraMove ();
		this.mouseSpeedX *= 0.95f;
		this.mouseSpeedY *= 0.95f;
		this.zoomIn ();

		if (slerping) {
			T = (Time.time - startTime) / journeyTime;	
			transform.position = Vector3.Slerp (curentPos, endPos,  Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f	, T)));
			this.cameraLook ();
			if(T > 1){
				slerping  = false;
			}
		}


	}

	private void cameraLook(){
		//Vector3 lookRot =  target.position - this.transform.position;
		this.transform.LookAt (target, transform.up);
	}

	private void cameraMove(){
		this.transform.RotateAround(target.position, transform.up, mouseSpeedX*speed);
		this.transform.RotateAround(target.position, -transform.right, mouseSpeedY*speed);
	}

	private float getMouseSpeedX(){
		return Input.GetAxis ("Mouse X");
	}
	private float getMouseSpeedY(){
		return Input.GetAxis ("Mouse Y");
	}

	private void zoomIn(){
		this.camera.orthographicSize -= Input.GetAxis ("Mouse ScrollWheel");
	}

	public void rotateToUnit(Transform basic){
		startTime = Time.time;
		curentPos = transform.position - target.position;
		targetPos = basic.transform.position - target.position;
		endPos = Vector3.Distance(target.position, transform.position) * targetPos.normalized;
		slerping = true;
	}
}
