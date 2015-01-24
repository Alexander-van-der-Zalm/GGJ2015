using UnityEngine;
using System.Collections;

public class Movement_Indicator : MonoBehaviour {

	public float newScale = 0.05f;
	public float xSpeed = 0;
	public float ySpeed = 0;

	public Material mat;

	public bool reducing = false;

	public GameObject[] faces;
	public GameObject child;


	// Use this for initialization
	void Start () {
		xSpeed = Random.Range (50, 150);
		ySpeed = Random.Range (50, 150);
		mat.color = new Color (mat.color.r, mat.color.g, mat.color.b, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!reducing) {
			this.incSize ();
		}
		this.rotate();
		this.fadeOut ();
		StartCoroutine (grow ());
		if (reducing) {
			this.redSize();
			StartCoroutine(destroy());
		}
	}

	public void incSize(){
		newScale += 0.014f;
		this.transform.localScale = new Vector3 (newScale, newScale, newScale);
	}

	public void redSize(){
		newScale -= 0.014f;
		this.transform.localScale = new Vector3 (newScale, newScale, newScale);
	}

	public void rotate(){
		this.transform.RotateAround (this.transform.position, this.transform.up, xSpeed*Time.deltaTime);
		this.transform.RotateAround (this.transform.position, this.transform.right, ySpeed*Time.deltaTime);
	}

	public void fadeOut(){
		float tempAlpha = mat.color.a - 0.004f;
		mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, tempAlpha);
	}

	IEnumerator grow(){
		yield return new WaitForSeconds(0.25f);
		reducing = true;
	}

	IEnumerator destroy(){
		yield return new WaitForSeconds(0.25f);
		Destroy (gameObject);
	}
}

