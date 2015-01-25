using UnityEngine;
using System.Collections;

public class MaterialManager : Singleton<MaterialManager> {


	public Material redTeam;
	public Material blueTeam;
	public Material neutral;

	public Material[] matIndex;
	public int size;

	void Start(){
		size = matIndex.Length;
	}
}
