using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonData : MonoBehaviour {

	public int Team = 1;//0 = blue; 1 = neutral; 2 = red;
	public int Beast = 0;// Beast coresponds to the type of button and what graphic it needs to load. Also interacts with the ID and removal of other buttons in different groups with the beast in it.

	public Text btn_text;
	public Image img;

	// Use this for initialization
	void Start () {
		btn_text.text = "" + Beast;
		img.color = getColor (Team);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Color getColor(int i){
		return new Color();
	}
}
  